using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class RakeBuildTool : IBuildTool
    {
        private readonly IEnvironmentVariable _environmentVariable;
        string directory = null;

        public RakeBuildTool(IEnvironmentVariable environmentVariable)
        {
            _environmentVariable = environmentVariable;
        }

        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            return string.Format("{0} --rakefile {1} {2}", Path.Combine(GetRubyDirectory(), "rake"), pathToBuildFile, GenerateTasks(buildEngine.Tasks));
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return Path.Combine(GetRubyDirectory(), "ruby.exe");
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            return version.ToString();
        }

        private string GenerateTasks(List<string> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return string.Empty;

            var ret = "";

            tasks.ForEach(x => ret += string.Format("{0} ", x));

            return ret;
        }


        private string GetRubyDirectory()
        {
            if (string.IsNullOrEmpty(directory))
                directory = _environmentVariable.GetDirectoryFor("ruby.exe");

            if (string.IsNullOrEmpty(directory))
            {
                throw new Exception("Ruby not installed or it's not registerred in the Environment Variables > path");
            }

            return directory;
        }
    }
}
