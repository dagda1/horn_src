using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Horn.Framework.helpers
{
    public static class DirectoryHelper
    {
        public const string GuidExpression =
    @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
        
        public static string GetTempDirectoryName()
        {
            var hornTemp = Path.Combine(Environment.GetEnvironmentVariable("temp"), "horntemp");

            var temp = new DirectoryInfo(Path.Combine(hornTemp, Guid.NewGuid().ToString()));

            InitialiseTempTreeFolder(new DirectoryInfo(hornTemp));

            var packageRoot = new DirectoryInfo(Path.Combine(temp.FullName, ".horn"));

            packageRoot.Create();

            return packageRoot.FullName;
        }

        public static void DeleteGuidDirectories(DirectoryInfo root)
        {
            foreach (var directory in root.GetDirectories())
            {
                if (!Regex.IsMatch(directory.Name, GuidExpression))
                    continue;

                try
                {
                    directory.Delete(true);
                }
                catch
                {
                    continue;
                }
            }            
        }

        public static string GetBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static void InitialiseTempTreeFolder(DirectoryInfo tempTreeRootFolder)
        {
            if (!tempTreeRootFolder.Exists)
                tempTreeRootFolder.Create();

            DeleteGuidDirectories(tempTreeRootFolder);

            var revisionDataFile = new FileInfo(Path.Combine(tempTreeRootFolder.FullName, "revision.horn"));

            if (revisionDataFile.Exists)
                revisionDataFile.Delete();
        }
    }
}