using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Horn.Core.Extensions;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Core.Utils.CmdLine;
using Horn.Core.Utils.IO;
using Horn.Services.Core.Config;
using horn.services.core.Value;
using log4net;
using Package=horn.services.core.Value.Package;

namespace Horn.Services.Core.Builder
{
    public class SiteStructureBuilder : ISiteStructureBuilder
    {
        private readonly IMetaDataSynchroniser metaDataSynchroniser;
        private readonly IFileSystemProvider fileSystemProvider;
        private readonly DirectoryInfo dropDirectory;
        private IPackageTree rootPackageTree;

        private bool hasRanOnce;

        protected DateTime nextPollTime;
        protected TimeSpan frequency;
        protected static readonly ILog log = LogManager.GetLogger(typeof(SiteStructureBuilder));

        //HACK: Temporary measure to get up and running
        private readonly string[] excludePackages = new[] { "cms", "viewengines", "json", "languages", "castle", "network", "boo", "n2cms", "masstransit", "network", "network", "dndns", "hasic", "moq", "json.net", "hasic", "sharp.architecture", "castle.nvelocity", "castle.templateengine" };
        private DirectoryInfo rootDirectory;

        public virtual List<Category> Categories { get; private set; }

        public virtual bool ServiceStarted { get; set; }

        public virtual bool ShouldContinueAfterException
        {
            get { return true; }
        }

        public virtual void Initialise()
        {
            rootDirectory = fileSystemProvider.GetHornRootDirectory(HornServiceConfig.Settings.HornRootDirectory);

            metaDataSynchroniser.SynchronisePackageTree(new PackageTree(rootDirectory, null));

            rootPackageTree = new PackageTree(rootDirectory, null);
        }

        public virtual void Build()
        {
            log.Info("in build.");

            var root = new Category(null, rootPackageTree);

            var parentDirectory = CreatePackageDirectory(root, dropDirectory, rootPackageTree);

            BuildCategories(rootPackageTree, root, parentDirectory);

            Categories.Add(root);

            CreateWebStructure(root);
        }

        public virtual void Run()
        {
            while (ServiceStarted)
            {
                if (hasRanOnce)
                {
                    SuspendTask();

                    if (!ServiceStarted)
                        break;
                }

                try
                {
                    Initialise();

                    Build();
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                    throw;
                }

                hasRanOnce = true;
            }
        }

        public virtual void BuildAndZipPackage(IPackageTree root, IFileSystemProvider fileSystemProvider, Package package, DirectoryInfo newDirectory, DirectoryInfo tempDirectory)
        {
            BuildPackage(package, newDirectory);

            package.SetContents(root.Result);

            //DeleteOldZipFiles(newDirectory);

            var zipFile = fileSystemProvider.ZipFolder(root.Result, newDirectory, package.FileName);

            package.ZipFileName = new PackageFile(zipFile);
        }

        protected virtual void BuildPackage(Package package, DirectoryInfo newDirectory)
        {
            var version = (package.IsTrunk) ? null : package.Version;

            if (!IoC.HasComponent <ICommandArgs>())
            {
                var commandArgs = new CommandArgs(package.Name, false, version, false, null);

                IoC.AddComponentInstance(CommandArgs.IoCKey, typeof(ICommandArgs), commandArgs); 
            }
            else
            {
                ((CommandArgs) IoC.Resolve<ICommandArgs>()).SetArguments(package.Name, false, version,
                                                                                           false, null);
            }

            var packageBuilder = IoC.Resolve<IPackageCommand>("install");

            //we are rebuilding the package tree each time to reset all the version numbers.
            var cleanPackageTree = new PackageTree(rootDirectory, null);

            packageBuilder.Execute(cleanPackageTree);
        }

        protected virtual void BuildCategories(IPackageTree packageTree, Category parent, DirectoryInfo parentDirectory)
        {           
            foreach (var childTree in packageTree.Children)
            {
                if(IsExcludedName(childTree))
                    continue;

                var childCategory = new Category(parent, childTree);

                var newDirectory = CreatePackageDirectory(childCategory, parentDirectory, childTree);

                BuildCategories(childTree, childCategory, newDirectory);

                parent.Categories.Add(childCategory);
            }
        }

        protected virtual void DeleteOldZipFiles(DirectoryInfo directory)
        {
            var oldZips = fileSystemProvider.GetFiles(directory, "*.zip");

            if(oldZips.Length <= 1)
                return;

            Array.Sort(oldZips, new FileInfoCompare());

            var files = new List<FileInfo>(oldZips);

            files.RemoveAt(0);  //do not delete the most current file

            foreach (var file in files)
            {
                try
                {
                    fileSystemProvider.DeleteFile(file.FullName);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        private bool IsExcludedName(IPackageTree childTree)
        {
            //if ((childTree.Name.ToLower() == "testing") || (childTree.Name.ToLower() == "mspec") || (childTree.Name.ToLower() == "mvccontrib"))
            //{
            //    Debugger.Break();

            //    return false;
            //}

            //return true;

            if (!string.IsNullOrEmpty(excludePackages.Where(x => x.ToLower() == childTree.Name.ToLower()).FirstOrDefault()))
                return true;

            return (childTree.Name.ToLower().IndexOf("working-") > -1);
        }

        private void CreateErrorTextFile(Exception exception, Package package, DirectoryInfo directory)
        {
            var tempFileName = Path.Combine(directory.FullName, string.Format("{0}.error", package.FileName));

            var error = string.Format("{0}\n", exception.Message);

            Exception innerException = exception.InnerException;

            while (innerException != null)
            {
                error = string.Format("{0}\n", innerException.Message);

                innerException = innerException.InnerException;
            }

            fileSystemProvider.WriteTextFile(tempFileName, error);
        }

        protected virtual DirectoryInfo CreatePackageDirectory(Category category, DirectoryInfo directory, IPackageTree packageTree)
        {
            var newDirectory = new DirectoryInfo(Path.Combine(directory.FullName, category.Name));

            fileSystemProvider.CreateDirectory(newDirectory.FullName);
          
            if(packageTree.IsBuildNode)
            {
                foreach (var package in category.Packages)
                {                        
                    hasRanOnce = true;

                    try
                    {
                        BuildAndZipPackage(rootPackageTree, fileSystemProvider, package, newDirectory, dropDirectory);    
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                         
                        package.IsError = true;

                        package.ErrorMessage = ex.UnwrapException();

                        CreateErrorTextFile(ex, package, newDirectory);
                    }                
                }
            }

            return newDirectory;
        }

        protected virtual void CreateWebStructure(Category root)
        {
            log.Info("Creating web structure");

            var xml = root.ToDataContractXml<Category>();

            var hornFile = Path.Combine(Path.Combine(dropDirectory.FullName, PackageTree.RootPackageTreeName), "horn.xml");

            log.InfoFormat("Writing xml to {0}", hornFile);

            fileSystemProvider.WriteTextFile(hornFile, xml);

            var resultXml = Path.Combine(HornServiceConfig.Settings.XmlLocation, "horn.xml");

            log.InfoFormat("Copying xml file to {0}", resultXml);

            fileSystemProvider.CopyFile(hornFile, resultXml, true);
        }

        protected virtual void SuspendTask()
        {
            Thread.Sleep(frequency);
        }

        public SiteStructureBuilder(IMetaDataSynchroniser metaDataSynchroniser, IFileSystemProvider fileSystemProvider, string dropDirectoryPath)
        {
            this.metaDataSynchroniser = metaDataSynchroniser;
            this.fileSystemProvider = fileSystemProvider;
            dropDirectory = new DirectoryInfo(dropDirectoryPath);
            Categories = new List<Category>();

            frequency = new TimeSpan(0, 0, HornServiceConfig.Settings.BuildFrequency, 0);
        }
    }
}