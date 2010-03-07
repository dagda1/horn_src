using System.Collections;
using System.IO;

namespace Horn.Core.Utils
{
    /// <summary>
    /// Basic wrapper for the file system to aid testing.  We don't want to hit the file system in the unit tests.
    /// Keep that for the integration tests
    /// </summary>
    public interface IFileSystemProvider
    {
        void CreateDirectory(string path);

        void CopyDirectory(string source, string destination);

        void CopyFile(string source, string destination, bool overwrite);

        DirectoryInfo CreateTemporaryHornDirectory(string path);

        void DeleteDirectory(string path);

        void DeleteFile(string path);

        bool Exists(string path);

        FileInfo[] GetFiles(DirectoryInfo directory, string pattern);

        DirectoryInfo GetTemporaryBuildDirectory(DirectoryInfo tempDirectory);

        DirectoryInfo GetHornRootDirectory(string path);

        void MkDir(string path);

        void MkFile(string path);

        void WriteTextFile(string destination, string text);

        FileInfo ZipFolder(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string packageName);
    }
}