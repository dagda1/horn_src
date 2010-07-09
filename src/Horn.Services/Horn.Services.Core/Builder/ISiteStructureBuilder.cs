using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;
using horn.services.core.Value;

namespace Horn.Services.Core.Builder
{
    public interface ISiteStructureBuilder
    {
        void Build();

        void BuildAndZipPackage(IPackageTree root, IFileSystemProvider fileSystemProvider, Package package, DirectoryInfo newDirectory,
                                DirectoryInfo tempDirectory);

        void Initialise();

        void Run();

        bool ServiceStarted { get; set; }
    }
}
