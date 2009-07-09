using System;
using System.IO;

namespace Horn.Core.PackageStructure
{
    public class BuildFileResolver : IBuildFileResolver
    {
        private string buildFile;

        public string BuildFile
        {
            get { return buildFile; }
        }

        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(buildFile))
                    throw new Exception("The file path has not been set for the BuildFileResolver");

                return Path.GetExtension(buildFile).Substring(1);
            }
        }

        public BuildFileResolver Resolve(DirectoryInfo buildFolder, string fileName)
        {
            buildFile = Path.Combine(buildFolder.FullName,
                                      string.Format("{0}.{1}", fileName, "boo"));

            if (!File.Exists(buildFile))
                  throw new MissingBuildFileException(buildFolder);

            return this;
        }
    }
}