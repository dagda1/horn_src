using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
	public class PhantomBuildTool : IBuildTool
	{
		public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
		{
			return string.Format(" -f:{0} {1}", pathToBuildFile, GenerateTasks(buildEngine.Tasks)).Trim();
		}

		public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
		{
			switch (version)
			{
				case FrameworkVersion.FrameworkVersion2:
					return "2.0";
				case FrameworkVersion.FrameworkVersion35:
					return "3.5";
			}

			throw new InvalidEnumArgumentException("Invalid Framework Version", (int)version, typeof(FrameworkVersion));
		}

		public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
		{
			var path = Path.Combine(packageTree.Root.CurrentDirectory.FullName, "buildengines");
			path = Path.Combine(path, "Phantom");
			path = Path.Combine(path, "Phantom.exe");

			return new FileInfo(path).FullName;
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
	}
}