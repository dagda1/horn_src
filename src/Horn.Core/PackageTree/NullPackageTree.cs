using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageStructure
{
    public class NullPackageTree : IPackageTree
    {
        public event BuildNodeCreatedHandler BuildNodeCreated;

        public void Add(IPackageTree item)
        {
            if (BuildNodeCreated != null)
                BuildNodeCreated(item);

            throw new NullTreeException();
        }

        public void Remove(IPackageTree item)
        {
            throw new NullTreeException();
        }

        public IPackageTree Parent
        {
            get { throw new NullTreeException(); }
            set { throw new NullTreeException(); }
        }

        public IPackageTree[] Children
        {
            get { throw new NullTreeException(); }
        }

        public string BuildFile
        {
            get { throw new NullTreeException(); }
        }

        public void BuildTree(IPackageTree parent, DirectoryInfo directory)
        {
            throw new NullTreeException();
        }

        public IBuildMetaData BuildMetaData
        {
            get { throw new NullTreeException(); }
        }

        public DirectoryInfo CurrentDirectory
        {
            get { throw new NullTreeException(); }
        }

        public bool Exists
        {
            get { throw new NullTreeException(); }
        }

        public string FullName
        {
            get { throw new NullTreeException(); }
        }

        public bool IsAversionRequest
        {
            get { throw new NullTreeException(); }
        }

        public bool IsBuildNode
        {
            get { throw new NullTreeException(); }
        }

        public bool IsRoot
        {
            get { throw new NullTreeException(); }
        }

        public string Name
        {
            get { throw new NullTreeException(); }
        }

        public FileInfo Nant
        {
            get { throw new NullTreeException(); }
        }

        public DirectoryInfo OutputDirectory
        {
            get { throw new NullTreeException(); }
        }

        public DirectoryInfo PatchDirectory
        {
            get { throw new NullTreeException(); }
        }

        public bool PatchExists
        {
            get { throw new NullTreeException(); }
        }

        public DirectoryInfo Result
        {
            get { throw new NullTreeException(); }
        }

        public IPackageTree Root
        {
            get { throw new NullTreeException(); }
        }

        public FileInfo Sn
        {
            get { throw new NullTreeException(); }
        }

        public DirectoryInfo WorkingDirectory
        {
            get { throw new NullTreeException(); }
        }

        public string Version
        {
            get { throw new NullTreeException(); }
            set { throw new NullTreeException(); }
        }

        public void DeleteWorkingDirectory()
        {
            throw new NullTreeException();
        }

        public List<IPackageTree> BuildNodes()
        {
            throw new NullTreeException();
        }

        public bool CannotAddThisDirectory(IPackageTree packageTreeNode, string[] reservedNames)
        {
            throw new NullTreeException();
        }

        public event CategoryNodeCreated CategoryCreated;

        public virtual void OnCategoryCreated(IPackageTree packageTreeNode)
        {
            if (CategoryCreated != null)
                CategoryCreated(packageTreeNode);
        }

        public List<IBuildMetaData> GetAllPackageMetaData()
        {
            throw new NullTreeException();
        }

        public IBuildMetaData GetBuildMetaData()
        {
            return GetBuildMetaData(Name);
        }

        public IBuildMetaData GetBuildMetaData(string packageName)
        {
            return new NullBuildMetaData();
        }

        public IRevisionData GetRevisionData()
        {
            throw new NullTreeException();
        }

        public IPackageTree GetRootPackageTree(DirectoryInfo rootFolder)
        {
            throw new NullTreeException();
        }

        public void PatchPackage()
        {
            throw new NullTreeException();
        }

        public IPackageTree RetrievePackage(string packageName)
        {
            throw new NullTreeException();
        }

        public IPackageTree RetrievePackage(Dependency dependency)
        {
            throw new NullTreeException();
        }

        public IPackageTree RetrievePackage(PackageArgs packageArgs)
        {
            throw new NullTreeException();
        }
    }
}