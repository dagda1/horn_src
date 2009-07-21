using System;
using System.IO;

namespace Horn.Framework.helpers
{
    public static class PackageTreeHelper
    {
        public const string PackageWithoutRevision = "norevisionpackage";
        public const string PackageWithRevision = "log4net";
        public const string PackageWithPatch = PackageWithRevision;

        public static DirectoryInfo CreateEmptyDirectoryStructureForTesting()
        {
            var rootDirectory = DirectoryHelper.GetTempDirectoryName();

            CreateDirectory(rootDirectory);

            return new DirectoryInfo(rootDirectory);
        }

        public static DirectoryInfo CreateDirectoryStructureForTesting()
        {
            var rootDirectory = DirectoryHelper.GetTempDirectoryName();

            CreateDirectory(rootDirectory);

            string builders = CreateDirectory(rootDirectory, "builders");
            string horn = CreateDirectory(builders, "horn");

            string hornFile = Path.Combine(DirectoryHelper.GetBaseDirectory(), @"BuildConfigs\Horn\horn.boo");
            string buildFile = Path.Combine(DirectoryHelper.GetBaseDirectory(), @"BuildConfigs\Horn\horn.boo");

            string fileToCopy = File.Exists(hornFile) ? hornFile : buildFile;

            CreateBuildFiles(fileToCopy, horn, true, false);

            string loggers = CreateDirectory(rootDirectory, "loggers");
            string log4net = CreateDirectory(loggers, "log4net");

            CreatePatchDirectory(log4net);

            var log4NetBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\log4net.boo");

            CreateBuildFiles(log4NetBuildFile, log4net, true, true);

            string ioc = CreateDirectory(rootDirectory, "ioc");
            string castle = CreateDirectory(ioc, "castle");

            string working = CreateDirectory(castle, "working");
            CreateTempBuildStructure(working);
            CreateDirectory(working, "Tools");

            var castleBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\castle.boo");
            var castleVersionBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\castle-2.1.0.boo");

            File.Copy(castleVersionBuildFile, Path.Combine(castle, "castle-2.1.0.boo"));

            CreateBuildFiles(castleBuildFile, castle, true, false);

            string tests = CreateDirectory(rootDirectory, "tests");
            string norevisionpackage = CreateDirectory(tests, PackageWithoutRevision);

            CreateBuildFiles(log4NetBuildFile, norevisionpackage, false, true);

            CreateBuildEnginesStructure(rootDirectory);

            return new DirectoryInfo(rootDirectory);
        }

        public static void CreatePatchDirectory(string rootFolder)
        {
            const string patchFile = "patchfile.txt";

            var patchDirectory = CreateDirectory(Path.Combine(rootFolder, "patch"));

            var childDirectory = CreateDirectory(Path.Combine(patchDirectory, "child"));

            var sourceFile = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BuildConfigs\\Horn"), patchFile);

            var destinationFile = Path.Combine(childDirectory, patchFile);

            File.Copy(sourceFile, destinationFile, true);
        }

        public static void CreateBuildFiles(string sourceFile, string destinationFolder, bool createRevisionFile, bool createVersionedRevisionFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException(string.Format("The build file {0} does not exist", sourceFile));

            string destinationBuildFile = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));

            File.Copy(sourceFile, destinationBuildFile, true);

            if (!createRevisionFile)
                return;

            var revisionFile = Path.Combine(destinationFolder, "revision.horn");

            var versionedRevisionFile = Path.Combine(destinationFolder, "revision-2.1.0.horn");

            CreateRevisionFile(revisionFile);

            if(createVersionedRevisionFile)
                CreateRevisionFile(versionedRevisionFile);
        }

        private static void CreateRevisionFile(string revisionFile)
        {
            using(var streamWriter = new StreamWriter(revisionFile))
            {
                streamWriter.Write("revision=1");

                streamWriter.Close();
            }
        }

        public static string CreateDirectory(string directoryPath, string newDirectoryName)
        {
            var combination = Path.Combine(directoryPath, newDirectoryName);

            return CreateDirectory(combination);
        }



        private static void CreateTempBuildStructure(string working)
        {
            string build = CreateDirectory(working, "build");
            string net = CreateDirectory(build, "net-3.5");
            string debug = CreateDirectory(net, "debug");

            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Horn.Core.dll");

            File.Copy(dllPath, Path.Combine(debug, "Horn.Core.dll"), true);
        }

        private static void CreateBuildEnginesStructure(string root)
        {
            var path = Path.Combine(root, "buildengines");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, "Nant");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, "Nant");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, "Nant.exe");
            var existingExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Nant.exe");
            File.Copy(existingExecutablePath, path, true);
        }

        private static string CreateDirectory(string directoryPath)
        {
            var directory = new DirectoryInfo(directoryPath);

            if(!directory.Exists)
                directory.Create();

            return directory.FullName;
        }



    }
}