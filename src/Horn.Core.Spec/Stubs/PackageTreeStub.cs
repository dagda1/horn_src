using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.Spec.Doubles
{
    public class PackageTreeStub : IPackageTree
    {
        private readonly IBuildMetaData buildMetaData;
        private readonly string name;
        private readonly bool useInternalDictionary;
        private readonly Dictionary<string, IPackageTree> dependencyTrees = new Dictionary<string, IPackageTree>();

        public string BaseDirectory
        {
            get; private set;
        }

        public string BuildFile
        {
            get { return "defaul.build"; }
        }

        public IBuildMetaData BuildMetaData
        {
            get { return buildMetaData; }
        }

        public IPackageTree[] Children
        {
            get { throw new NotImplementedException(); }
        }

        public DirectoryInfo CurrentDirectory
        {
            get { throw new NotImplementedException(); }
        }

        public bool Exists
        {
            get { throw new NotImplementedException(); }
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Version))
                    return Name;

                return string.Format("{0}-{1}", Name, Version);
            }
        }

        public bool IsAversionRequest
        {
            get { return string.IsNullOrEmpty(Version) == false; }
        }

        public bool IsBuildNode
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRoot
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return name; }
        }

        public FileInfo Nant
        {
            get { throw new NotImplementedException(); }
        }

        public DirectoryInfo OutputDirectory
        {
            get { return new DirectoryInfo(Path.Combine(BaseDirectory, "build_root_dir")); }
            set { throw new NotImplementedException(); }
        }

        public IPackageTree Parent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        private DirectoryInfo result;
        public DirectoryInfo Result
        {
            get
            {
                if (result == null)
                    return new DirectoryInfo(@"Z:\nowhere\");

                return result;
            }
            set { result = value; }
        }

        public IPackageTree Root
        {
            get { throw new NotImplementedException(); }
        }

        public FileInfo Sn
        {
            get { throw new NotImplementedException(); }
        }

        public DirectoryInfo WorkingDirectory
        {
            get { return new DirectoryInfo(Path.Combine(BaseDirectory, "working")).Parent; }
        }

        public string Version { get; set; }

        public void Add(IPackageTree item)
        {
            throw new NotImplementedException();
        }

        public void AddDependencyPackageTree(string dependencyName, PackageTreeStub dependencyTree)
        {
            dependencyTrees.Add(dependencyName, dependencyTree);
        }

        public List<IPackageTree> BuildNodes()
        {
            return new List<IPackageTree>{this};
        }

        public void CreateRequiredDirectories()
        {
            throw new NotImplementedException();
        }

        public void DeleteWorkingDirectory()
        {
            Console.WriteLine("Deleting working directory.");
        }

        public IBuildMetaData GetBuildMetaData(string packageName)
        {
            return buildMetaData;
        }

        public IRevisionData GetRevisionData()
        {
            throw new NotImplementedException();
        }

        public IPackageTree GetRootPackageTree(DirectoryInfo rootFolder)
        {
            throw new NotImplementedException();
        }

        public void Remove(IPackageTree item)
        {
            throw new NotImplementedException();
        }

        public IPackageTree RetrievePackage(string packageName)
        {
            if (!useInternalDictionary)
                return this;

            return dependencyTrees[packageName];
        }

        public IPackageTree RetrievePackage(Dependency dependency)
        {
            return RetrievePackage(dependency.PackageName);
        }

        public IPackageTree RetrievePackage(ICommandArgs commandArgs)
        {
            return RetrievePackage(commandArgs.PackageName);
        }

        public PackageTreeStub(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }

        public PackageTreeStub(IBuildMetaData buildMetaData, string name, bool useInternalDictionary)
        {
            this.buildMetaData = buildMetaData;
            this.name = name;
            this.useInternalDictionary = useInternalDictionary;
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}