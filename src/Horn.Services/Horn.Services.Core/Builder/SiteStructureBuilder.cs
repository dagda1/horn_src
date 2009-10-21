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
        private DirectoryInfo sandBox;

        private bool hasRanOnce;

        protected DateTime nextPollTime;
        protected TimeSpan frequency;
        protected static readonly ILog log = LogManager.GetLogger(typeof(SiteStructureBuilder));

        private string[] excludePackages = new string[]{"castle"};

        public virtual List<Category> Categories { get; private set; }

        public virtual bool ServiceStarted { get; set; }

        public virtual bool ShouldContinueAfterException
        {
            get { return true; }
        }

        public virtual void Initialise()
        {
            var rootDirectory = fileSystemProvider.GetHornRootDirectory(HornConfig.Settings.HornRootDirectory);

            metaDataSynchroniser.SynchronisePackageTree(new PackageTree(rootDirectory, null));

            rootPackageTree = new PackageTree(rootDirectory, null);

            sandBox = fileSystemProvider.CreateTemporaryHornDirectory(HornConfig.Settings.HornTempDirectory);
        }

        public virtual void Build()
        {
            log.Info("in build.");

            var root = new Category(null, rootPackageTree);

            var parentDirectory = CreatePackageDirectory(root, sandBox, rootPackageTree);

            BuildCategories(rootPackageTree, root, parentDirectory);

            Categories.Add(root);

            CreateWebStructure(root);
        }

        public virtual void Run()
        {
            var hasRanOnce = false;

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

        protected virtual void BuildAndZipPackage(IFileSystemProvider fileSystemProvider, Package package, DirectoryInfo newDirectory, DirectoryInfo tempDirectory)
        {
            BuildPackage(package, newDirectory);

            var zipFile = fileSystemProvider.ZipFolder(rootPackageTree.Result, newDirectory, package.FileName);

            package.SetContents(rootPackageTree.Result, zipFile);
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

            packageBuilder.Execute(rootPackageTree);
        }

        protected virtual void BuildCategories(IPackageTree packageTree, Category parent, DirectoryInfo parentDirectory)
        {           
            foreach (var childTree in packageTree.Children)
            {
                if(!string.IsNullOrEmpty(excludePackages.Where(x => x.ToLower() == childTree.Name.ToLower()).FirstOrDefault()))
                    continue;

                var childCategory = new Category(parent, childTree);

                var newDirectory = CreatePackageDirectory(childCategory, parentDirectory, childTree);

                BuildCategories(childTree, childCategory, newDirectory);

                parent.Categories.Add(childCategory);
            }
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
                    if(!hasRanOnce)
                    {
                        log.Info("Running for the first time.");

                        Debugger.Break();
                    }
                        
                    hasRanOnce = true;

                    try
                    {
                        BuildAndZipPackage(fileSystemProvider, package, newDirectory, sandBox);    
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);

                        CreateErrorTextFile(ex, package, newDirectory);
                    }                
                }
            }

            return newDirectory;
        }

        protected virtual void CreateWebStructure(Category root)
        {
            var xml = root.ToDataContractXml<Category>();

            var hornFile = Path.Combine(sandBox.FullName, "horn.xml");

            fileSystemProvider.WriteTextFile(hornFile, xml);

            Debugger.Break();

            var destinationDirectory = Path.Combine(dropDirectory.FullName, PackageTree.RootPackageTreeName);

            fileSystemProvider.CopyDirectory(sandBox.FullName, destinationDirectory);
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

            frequency = new TimeSpan(0, 0, HornConfig.Settings.BuildFrequency, 0);
        }
    }
}