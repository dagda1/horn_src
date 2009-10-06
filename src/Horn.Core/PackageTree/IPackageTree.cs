using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageStructure
{
    public interface IPackageTree : IComposite<IPackageTree>
    {
        string BuildFile { get; }

        void BuildTree(IPackageTree parent, DirectoryInfo directory);

        IBuildMetaData BuildMetaData { get; }

        event BuildNodeCreatedHandler BuildNodeCreated;

        List<IPackageTree> BuildNodes();

        event CategoryNodeCreated CategoryCreated;

        DirectoryInfo CurrentDirectory { get; }

        void DeleteWorkingDirectory();

        bool Exists { get; }

        string FullName { get; }

        List<IBuildMetaData> GetAllPackageMetaData();

        IBuildMetaData GetBuildMetaData();

        IBuildMetaData GetBuildMetaData(string packageName);

        IRevisionData GetRevisionData();

        IPackageTree GetRootPackageTree(DirectoryInfo rootFolder);

        bool IsAversionRequest { get; }

        bool IsBuildNode { get; }

        bool IsRoot { get; }

        string Name { get; }

        DirectoryInfo OutputDirectory { get; }

        void PatchPackage();

        DirectoryInfo PatchDirectory { get; }

        bool PatchExists { get; }

        DirectoryInfo Result { get; }

        IPackageTree RetrievePackage(string packageName);

        IPackageTree RetrievePackage(Dependency dependency);

        IPackageTree RetrievePackage(ICommandArgs commandArgs);

        IPackageTree Root { get; }

        string Version { get; set; }

        DirectoryInfo WorkingDirectory { get; }
    }
}