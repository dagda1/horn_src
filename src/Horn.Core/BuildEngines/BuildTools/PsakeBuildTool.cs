using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class PSakeBuildTool : IBuildTool
    {
        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            return string.Format(@"  -command .\psake {0} {1}", Path.GetFileName(pathToBuildFile.Trim('"')), GenerateTasks(buildEngine.Tasks));
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return "Powershell.exe";
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            return "PSake files don't care";
        }

        private string GenerateTasks(IEnumerable<string> tasks)
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