using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class RakeBuildTool : IBuildTool
    {
        private readonly IEnvironmentVariable environmentVariable;
        string directory;

        public RakeBuildTool(IEnvironmentVariable environmentVariable)
        {
            this.environmentVariable = environmentVariable;
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

        private string GenerateTasks(IEnumerable<string> tasks)
        {
            if( tasks == null )
            {
                return String.Empty;
            }

            var tasksArgument = String.Empty;
            tasks.ForEach( task => tasksArgument += String.Format("{0} ", task) );
            return tasksArgument;
        }


        private string GetRubyDirectory()
        {
            if (string.IsNullOrEmpty(directory))
                directory = environmentVariable.GetDirectoryFor("ruby.exe");

            if (string.IsNullOrEmpty(directory))
            {
                throw new Exception("Ruby not installed or it's not registerred in the Environment Variables > path");
            }

            return directory;
        }
    }
}
