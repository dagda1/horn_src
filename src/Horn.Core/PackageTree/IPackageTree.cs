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

        IBuildMetaData BuildMetaData { get; }

        DirectoryInfo CurrentDirectory { get; }

        bool Exists { get; }

        string FullName { get; }

        bool IsAversionRequest { get; }

        bool IsBuildNode { get; }

        bool IsRoot { get; }

        string Name { get; }

        FileInfo Nant { get; }

        DirectoryInfo OutputDirectory { get; }

        DirectoryInfo PatchDirectory { get; }

        bool PatchExists { get; }

        DirectoryInfo Result { get; }

        IPackageTree Root { get; }

        FileInfo Sn { get; }

        DirectoryInfo WorkingDirectory { get; }

        string Version { get; set; }

        void CreateRequiredDirectories();

        void DeleteWorkingDirectory();

        List<IPackageTree> BuildNodes();

        IBuildMetaData GetBuildMetaData(string packageName);

        IRevisionData GetRevisionData();

        IPackageTree GetRootPackageTree(DirectoryInfo rootFolder);

        void PatchPackage();

        IPackageTree RetrievePackage(string packageName);

        IPackageTree RetrievePackage(Dependency dependency);

        IPackageTree RetrievePackage(ICommandArgs commandArgs);
    }
}