using System;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class PSakeBuildTool : IBuildTool
    {
        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {
            return string.Format("  -command {0}", pathToBuildFile);
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return "Powershell.exe";
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            return "PSake files don't care";
        }
    }
}