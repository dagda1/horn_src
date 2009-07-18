using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageStructure
{
    public class PackageTree : IPackageTree
    {
        private readonly IMetaDataSynchroniser metaDataSynchroniser;
        private DirectoryInfo result;
        public const string RootPackageTreeName = ".horn";
        private readonly IList<IPackageTree> children;
        private DirectoryInfo workingDirectory;
        private readonly static string[] reservedDirectoryNames = new[]{"working", "build_root_dir"};
        private static readonly string[] libraryNodes = new[] {"lib", "debug", "buildengines"};

        public string BuildFile{ get; set; }

        public IBuildMetaData BuildMetaData{ get; private set; }

        public IPackageTree[] Children
        {
            get { return children.ToArray(); }
        }

        public DirectoryInfo CurrentDirectory { get; private set; }

        public bool Exists
        {
            get
            {
                if (!CurrentDirectory.Exists)
                    return false;

                return RootDirectoryContainsBuildFiles() > 0;
            }
        }

        public string FullName
        {
            get
            {
                if (!IsAversionRequest)
                    return Name;

                return string.Format("{0}-{1}", Name, Version);
            }
        }

        public bool IsAversionRequest
        {
            get { return string.IsNullOrEmpty(Version) == false; }
        }

        public bool IsBuildNode { get; private set; }

        public bool IsRoot
        {
            get { return (Parent == null); }
        }

        public string Name{ get; private set; }

        public FileInfo Nant
        {
            get
            {
                //TODO: Find a less explicit way to find the nant.exe
                var path = Path.Combine(Root.CurrentDirectory.FullName, "buildengines");
                path = Path.Combine(path, "Nant");
                path = Path.Combine(path, "Nant");
                path = Path.Combine(path, "NAnt.exe");

                return new FileInfo(path);
            }
        }

        public DirectoryInfo OutputDirectory { get; private set; }

        public IPackageTree Parent { get; set; }

        public DirectoryInfo Result
        {
            get
            {
                result = new DirectoryInfo(Path.Combine(Root.CurrentDirectory.FullName, "result"));

                if(!result.Exists)
                    result.Create();

                return result;
            }
        }

        public IPackageTree Root
        {
            get
            {
                if (IsRoot)
                    return this;

                IPackageTree parent = Parent;

                while (!parent.IsRoot)
                {
                    parent = parent.Parent;
                }

                return parent;
            }
        }

        public FileInfo Sn
        {
            get
            {
                //TODO: Find a less explicit way to find the sn.exe
                var path = Path.Combine(Root.CurrentDirectory.FullName, "buildengines");
                path = Path.Combine(path, "Sn");
                path = Path.Combine(path, "sn.exe");

                return new FileInfo(path);                
            }
        }

        public string Version { get; set; }

        public DirectoryInfo WorkingDirectory
        {
            get
            {
                if (IsRoot)
                    return CurrentDirectory;

                if(string.IsNullOrEmpty(Version)) 
                    return workingDirectory;

                var versionedDirectoryPath = string.Format("{0}-{1}", workingDirectory.FullName, Version);

                return new DirectoryInfo(versionedDirectoryPath);
            }

            private set { workingDirectory = value; }
        }

        public void Add(IPackageTree item)
        {
            item.Parent = this;

            children.Add(item);
        }

        public List<IPackageTree> BuildNodes()
        {
            var result = Root.GetAllPackages()
                .Where(c => c.IsBuildNode).ToList();

            return result;
        }

        public virtual void CreateRequiredDirectories()
        {
            WorkingDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, "Working"));
            WorkingDirectory.Create();

            OutputDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, "Output"));
            OutputDirectory.Create();
        }

        public virtual void DeleteWorkingDirectory()
        {
            if (!Root.Name.StartsWith(RootPackageTreeName))
                return;

            if(!WorkingDirectory.Exists)
                return;

            WorkingDirectory.Delete(true);
        }

        public IBuildMetaData GetBuildMetaData(string packageName)
        {
            IPackageTree packageTree = RetrievePackage(packageName);

            return GetBuildMetaData(packageTree);
        }

        public IRevisionData GetRevisionData()
        {
            return new RevisionData(this);
        }

        public IPackageTree GetRootPackageTree(DirectoryInfo rootFolder)
        {
            IPackageTree root = new PackageTree(rootFolder, null);

            metaDataSynchroniser.SynchronisePackageTree(root);

            return new PackageTree(rootFolder, null);            
        }

        public void Remove(IPackageTree item)
        {
            children.Remove(item);

            item.Parent = null;
        }

        public IPackageTree RetrievePackage(Dependency dependency)
        {
            return RetrievePackage(dependency.PackageName, dependency.Version);
        }

        public IPackageTree RetrievePackage(ICommandArgs commandArgs)
        {
            var packageName = commandArgs.PackageName;

            var version = commandArgs.Version;

            return RetrievePackage(packageName, version);
        }

        public IPackageTree RetrievePackage(string packageName)
        {
            return RetrievePackage(packageName, null);
        }

        private IPackageTree RetrievePackage(string packageName, string version)
        {
            var nodes = Root.GetAllPackages()
                .Where(c => c.Name == packageName).ToList();

            if (nodes.Count() == 0)
                return new NullPackageTree();

            var result = nodes.First();

            //HACK: Need a better way of initialising the package tree with the version information
            if (!string.IsNullOrEmpty(version))
                result.Version = version;

            return result;
        }

        private int RootDirectoryContainsBuildFiles()
        {
            return (WorkingDirectory.GetFiles("horn.*", SearchOption.AllDirectories).Length);
        }

        private IBuildMetaData GetBuildMetaData(IPackageTree packageTree)
        {
            if (BuildMetaData != null)
                return BuildMetaData;

            var buildFileResolver = new BuildFileResolver().Resolve(packageTree.CurrentDirectory, packageTree.FullName);

            var reader = IoC.Resolve<IBuildConfigReader>();

            BuildMetaData = reader.SetDslFactory(packageTree).GetBuildMetaData(packageTree, Path.GetFileNameWithoutExtension(buildFileResolver.BuildFile));

            return BuildMetaData;
        }

        private PackageTree CreateNewPackageTree(DirectoryInfo child)
        {
            return new PackageTree(child, this);
        }

        private bool IsReservedDirectory(DirectoryInfo child)
        {
            return reservedDirectoryNames.Contains(child.Name.ToLower());
        }

        private bool DirectoryIsBuildNode(DirectoryInfo directory)
        {
            if (IsRoot)
                return false;

            return (directory.GetFiles("*.boo").Length > 0) &&
                   (!libraryNodes.Contains(directory.Name.ToLower()));
        }

        public PackageTree(IMetaDataSynchroniser metaDataSynchroniser)
        {
            this.metaDataSynchroniser = metaDataSynchroniser;
        }

        public PackageTree(DirectoryInfo directory, IPackageTree parent)
        {
            Parent = parent;

            children = new List<IPackageTree>();

            Name = directory.Name;

            CurrentDirectory = directory;

            IsBuildNode = DirectoryIsBuildNode(directory);

            if(IsBuildNode)
            {
                CreateRequiredDirectories();
            }

            foreach (var child in directory.GetDirectories())
            {
                if (IsReservedDirectory(child))
                    return;

                children.Add(CreateNewPackageTree(child));
            }

            if (parent == null)
            {
                Result.Delete(true);
            }
        }
    }
}