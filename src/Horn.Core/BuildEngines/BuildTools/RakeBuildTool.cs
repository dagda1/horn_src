using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
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
			return string.Format("{0} --rakefile {1} {2}{3}", Path.Combine(GetRubyDirectory(), "rake"), pathToBuildFile, GenerateParameters(buildEngine.Parameters), GenerateTasks(buildEngine.Tasks)).Trim();
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return Path.Combine(GetRubyDirectory(), "rake.bat");
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

		private string GenerateParameters(IDictionary<string, string> parameters)
		{
			if (parameters == null || parameters.Keys.Count == 0)
				return String.Empty;

			var stringBuilder = new StringBuilder();

			foreach (var key in parameters.Keys)
				stringBuilder.AppendFormat("{0}={1} ", key, parameters[key]);

			return stringBuilder.ToString();
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
