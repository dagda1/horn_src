using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Horn.Core.PackageStructure
{
    public class BuildFileResolver : IBuildFileResolver
    {
        private string buildFile;
        private string _version;

        private const string VersionPattern = @"-[0-9]+\.[0-9]+\.";

        public string BuildFile
        {
            get { return buildFile; }
        }

        public string Version
        {
            get { return _version; }
        }

        public BuildFileResolver Resolve(DirectoryInfo buildFolder, string fileName)
        {
            buildFile = Path.Combine(buildFolder.FullName,
                                      string.Format("{0}.{1}", fileName, "boo"));

            if (!File.Exists(buildFile))
                throw new MissingBuildFileException(buildFolder);

            if (Regex.IsMatch(buildFile, VersionPattern, RegexOptions.IgnoreCase))
            {
                _version = fileName.Substring(fileName.LastIndexOf('-') + 1);

                return this;
            }

            _version = "trunk";

            return this;
        }
    }
}