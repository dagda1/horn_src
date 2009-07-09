using System;
using System.IO;

namespace Horn.Framework.helpers
{
    public static class PackageTreeHelper
    {

        public const string PACKAGE_WITHOUT_REVISION = "norevisionpackage";
        public  const string PACKAGE_WITH_REVISION = "log4net";


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

            CreateBuildFiles(fileToCopy, horn, true);

            string loggers = CreateDirectory(rootDirectory, "loggers");
            string log4net = CreateDirectory(loggers, "log4net");

            var log4NetBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\log4net.boo");

            CreateBuildFiles(log4NetBuildFile, log4net, true);

            string ioc = CreateDirectory(rootDirectory, "ioc");
            string castle = CreateDirectory(ioc, "castle");
            string working = CreateDirectory(castle, "working");
            CreateTempBuildStructure(working);
            CreateDirectory(working, "Tools");

            var castleBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\castle.boo");
            var castleVersionBuildFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BuildConfigs\Horn\castle-2.1.0.boo");

            File.Copy(castleVersionBuildFile, Path.Combine(castle, "castle-2.1.0.boo"));

            CreateBuildFiles(castleBuildFile, castle, true);

            string tests = CreateDirectory(rootDirectory, "tests");
            string norevisionpackage = CreateDirectory(tests, PACKAGE_WITHOUT_REVISION);

            CreateBuildFiles(log4NetBuildFile, norevisionpackage, false);

            CreateBuildEnginesStructure(rootDirectory);

            return new DirectoryInfo(rootDirectory);
        }

        public static void CreateBuildFiles(string sourceFile, string destinationFolder, bool createRevisionFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException(string.Format("The build file {0} does not exist", sourceFile));

            string destinationBuildFile = Path.Combine(destinationFolder, Path.GetFileName(sourceFile));

            File.Copy(sourceFile, destinationBuildFile, true);

            if (!createRevisionFile)
                return;

            var revisionFile = Path.Combine(destinationFolder, "revision.horn");

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