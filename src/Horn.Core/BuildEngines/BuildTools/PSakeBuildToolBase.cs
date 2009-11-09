using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
	public abstract class PSakeBuildToolBase : IBuildTool
	{
		public abstract string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version);

		public abstract string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version);

		public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
		{
			return "PSake files don't care";
		}

		protected string GeneratePSakeCommand(string pathToBuildFile, BuildEngine buildEngine)
		{
			return string.Format(@".\psake {0} {1}", Path.GetFileName(pathToBuildFile.Trim('"')), GenerateTasks(buildEngine.Tasks));
		}

		protected static string GenerateTasks(IEnumerable<string> tasks)
		{
			if (tasks == null)
			{
				return String.Empty;
			}

			var tasksArgument = String.Empty;
			tasks.ForEach(task => tasksArgument += String.Format("{0} ", task));
			return tasksArgument;
		}
	}
}