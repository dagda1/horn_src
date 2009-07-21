using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace Horn.Core.extensions
{
    public static class FileSystemInfoExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (FileSystemInfoExtensions));

        public static void CopyToDirectory(this DirectoryInfo source, DirectoryInfo destination, bool deleteDestination)
        {
            if(deleteDestination)
            {
                if (destination.Exists)
                    destination.SafeDelete();               
            }

            if(!destination.Exists)
                destination.Create();

            LogCopyTask(source, destination);

            CopyFiles(source, destination);

            CopyDirectories(source, destination, deleteDestination);
        }

        public static FileSystemInfo GetExportPath(string fullPath)
        {
            FileSystemInfo exportPath;

            if (fullPath.PathIsFile())
            {
                return new FileInfo(fullPath);
            }

            exportPath = new DirectoryInfo(fullPath);

            if (!exportPath.Exists)
            {
                var directoryInfo = (DirectoryInfo)exportPath;
                directoryInfo.Create();
            }

            return exportPath;
        }

        public static DirectoryInfo GetDirectoryFromParts(this FileSystemInfo source, string parts)
        {
            return (DirectoryInfo) GetFileSystemObjectFromParts(source, parts, false);
        }

        public static DirectoryInfo GetFileFromParts(this FileSystemInfo source, string parts)
        {
            return (DirectoryInfo)GetFileSystemObjectFromParts(source, parts, true);
        }

        public static FileSystemInfo GetFileSystemObjectFromParts(this FileSystemInfo source, string parts)
        {
            var outputPath = CorrectFilePath(parts, source);

            return outputPath.PathIsFile() ? (FileSystemInfo)new FileInfo(outputPath) : new DirectoryInfo(outputPath);
        }

        public static FileSystemInfo GetFileSystemObjectFromParts(this FileSystemInfo source, string parts, bool isFile)
        {            
            var outputPath = CorrectFilePath(parts, source);

            return (isFile) ? (FileSystemInfo)new FileInfo(outputPath) : new DirectoryInfo(outputPath);
        }

        public static IEnumerable<string> Search(this DirectoryInfo root, string searchPattern)
        {
            var dirs = new Queue<string>();
            dirs.Enqueue(root.FullName);

            var results = new List<string>();
            while (dirs.Count > 0)
            {
                SearchDirectories(searchPattern, dirs, results);
            }

            return results;
        }

        public static bool IsFile(this FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.FullName.PathIsFile());
        }

        public static bool PathIsDirectory(this string fullPath)
        {
            bool isDir = (File.GetAttributes(fullPath) & FileAttributes.Directory) == FileAttributes.Directory;

            return (isDir);
        }

        public static bool PathIsFile(this string fullPath)
        {
            return (!fullPath.PathIsDirectory());
        }



        private static void CopyDirectories(DirectoryInfo source, DirectoryInfo destination, bool deleteDestination)
        {
            foreach (var dir in source.GetDirectories())
            {
                if (dir.FullName.ToLower().Contains(".svn"))
                    continue;

                var newDirectory = new DirectoryInfo(Path.Combine(destination.FullName, dir.Name));

                LogCopyTask(dir, newDirectory);

                dir.CopyToDirectory(newDirectory, deleteDestination);
            }
        }

        private static void CopyFiles(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var file in source.GetFiles())
            {
                var destinationFile = new FileInfo(Path.Combine(destination.FullName, Path.GetFileName(file.FullName)));

                LogCopyTask(source, destination);

                if(!destinationFile.Directory.Exists)
                    destinationFile.Directory.Create();

                file.CopyTo(destinationFile.FullName, true);
            }
        }

        private static void SafeDelete(this DirectoryInfo source)
        {
            try
            {
                source.Delete(true);
            }
            catch
            {               
            }
            
        }

        private static void LogCopyTask(FileSystemInfo source, FileSystemInfo destination)
        {
            if ((source.FullName.Length) > 240 || (destination.FullName.Length > 240))
                return;

            log.InfoFormat("Copying from {0} to {1}", source.FullName, destination.FullName);
        }

        private static string[] AddFiles(string dir, string searchPattern, string[] filePaths, List<string> results)
        {
            // I do not like swallowing exceptions but there is a good reason
            // http://msdn.microsoft.com/en-us/library/bb513869.aspx
            try
            {
                filePaths = Directory.GetFiles(dir, searchPattern);
            }
            catch
            {
            }

            if (filePaths != null && filePaths.Length > 0)
            {
                foreach (string file in filePaths)
                {
                    results.Add(file);
                }
            }

            return filePaths;
        }

        private static string CorrectFilePath(string parts, FileSystemInfo source)
        {
            if (string.IsNullOrEmpty(parts))
                return source.FullName;

            if (parts.Trim() == "." && (!source.IsFile()))
                return Path.Combine(((DirectoryInfo) source).Parent.FullName, "Output");

            var outputPath = source.FullName;

            foreach (var part in parts.Split('/'))
            {
                outputPath = Path.Combine(outputPath, part);
            }

            return outputPath;
        }

        private static void SearchDirectories(string searchPattern, Queue<string> directories, List<string> results)
        {
            string dir = directories.Dequeue();

            string[] filePaths = null;

            filePaths = AddFiles(dir, searchPattern, filePaths, results);

            string[] directoryPaths = null;

            try
            {
                directoryPaths = Directory.GetDirectories(dir);
            }
            catch
            {
            }

            if (directoryPaths != null && directoryPaths.Length > 0)
            {
                foreach (string subDir in directoryPaths)
                {
                    directories.Enqueue(subDir);
                }
            }
        }

    }
}
