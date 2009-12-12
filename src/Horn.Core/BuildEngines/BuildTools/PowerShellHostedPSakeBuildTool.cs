using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
    public class PowerShellHostedPSakeBuildTool : PSakeBuildToolBase
    {
        public override string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
        {			
            return string.Format(@"  -command {0}", GeneratePSakeCommand(pathToBuildFile, buildEngine));
        }

        public override string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return "Powershell.exe";
        }     
    }
}