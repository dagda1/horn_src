using System;
using System.IO;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace Horn.Core.Utils
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public const string FileDateFormat = "dd-MM-yy-HHmmss";

        private static readonly ILog log = LogManager.GetLogger(typeof (FileSystemProvider));

        public virtual void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public virtual void CopyDirectory(string source, string destination)
        {
            Directory.Move(source, destination);
        }

        public virtual void CopyFile(string source, string destination, bool overwrite)
        {
            File.Copy(source, destination, overwrite);
        }

        public virtual DirectoryInfo CreateTemporaryHornDirectory(string path)
        {
            var tempDirectory = new DirectoryInfo(Path.Combine(path, "horn"));

            if(tempDirectory.Exists)
            {
                try
                {
                    tempDirectory.Delete(true);
                }
                catch(Exception ex)
                {
                    throw new CannotDeleteTempHornDirectoryException(string.Format("A problem has occurred deleting the horn directory {0}", tempDirectory.FullName), ex);
                }
            }

            tempDirectory.Create();

            return tempDirectory;
        }

        public virtual void Delete(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.IsFile())
            {
                fileInfo.Delete();

                return;
            }

            var directoryInfo = new DirectoryInfo(path);

            directoryInfo.Delete(true);
        }

        public virtual void DeleteDirectory(string path)
        {
            if (!Exists(path))
                return;

            Directory.Delete(path, true);
        }

        public virtual void DeleteFile(string path)
        {
            if (!Exists(path))
                return;

            File.Delete(path);
        }

        public virtual bool Exists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public virtual FileInfo[] GetFiles(DirectoryInfo directory, string pattern)
        {
            return directory.GetFiles(pattern);
        }

        public DirectoryInfo GetHornRootDirectory(string path)
        {
            var hornPath = new DirectoryInfo(path);

            var root = new DirectoryInfo(Path.Combine(hornPath.FullName, PackageTree.RootPackageTreeName));  

            if(!root.Exists)
                root.Create();

            return root;
        }

        public DirectoryInfo GetTemporaryBuildDirectory(DirectoryInfo tempDirectory)
        {
            var tempBuildDirectory = new DirectoryInfo(Path.Combine(tempDirectory.FullName, "Build"));

            return tempBuildDirectory;
        }

        public virtual void MkDir(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            directoryInfo.Create();
        }

        public virtual void MkFile(string path)
        {
            var fileInfo = new FileInfo(path);

            fileInfo.Create();
        }       

        public virtual void WriteTextFile(string destination, string text)
        {
            using(var fs = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var streamWriter = new StreamWriter(fs);
                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.Write(text);
                streamWriter.Flush();
            }
        }

        public virtual FileInfo ZipFolder(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string packageName)
        {
            var zipFileName = Path.Combine(targetDirectory.FullName, string.Format("{0}-{1}.zip", packageName, DateTime.Now.ToString(FileDateFormat)));

            ZipFolder(sourceDirectory, zipFileName);

            try
            {
                sourceDirectory.Delete(true);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return new FileInfo(zipFileName);
        }

        private void ZipFolder(DirectoryInfo tempFolder, string zipFileName)
        {
            using (var zipOutputStream = new ZipOutputStream(File.Create(zipFileName)))
            {
                zipOutputStream.SetLevel(9);

                var buffer = new byte[4096];

                foreach (var file in  tempFolder.GetFiles())
                {
                    var entry = new ZipEntry(Path.GetFileName(file.FullName))
                                    {
                                        DateTime = DateTime.Now
                                    };

                    zipOutputStream.PutNextEntry(entry);

                    using (var fs = File.OpenRead(file.FullName))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            zipOutputStream.Write(buffer, 0, sourceBytes);
                        }
                        while (sourceBytes > 0);
                    }
                }

                zipOutputStream.Finish();
                zipOutputStream.Close();
            }
        }
    }
}