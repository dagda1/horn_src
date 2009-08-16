using System;
using System.Text;
using Horn.Core.BuildEngines;
using Horn.Core.extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using System.IO;

namespace Horn.Core
{
    public class MSBuildBuildTool : IBuildTool
    {
        public string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree,
                        FrameworkVersion version)
        {
            var cmdLine = new StringBuilder();

            cmdLine.AppendFormat(
                     "{0} /p:OutputPath=\"{1}\"  /p:TargetFrameworkVersion={2} /p:NoWarn=1591 /consoleloggerparameters:Summary /target:{3}",
                     pathToBuildFile.QuotePath(), Path.Combine(packageTree.WorkingDirectory.FullName, buildEngine.BuildRootDirectory),
                     GetFrameworkVersionForBuildTool(version), String.Join(",", buildEngine.Tasks.ToArray()));

            foreach(var parameter in buildEngine.Parameters)
            {
                cmdLine.AppendFormat(" {0}={1}", parameter.Key, parameter.Value);
            }

            return cmdLine.ToString();
        }

        public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            switch (version)
            {
                case FrameworkVersion.FrameworkVersion2:
                    return "v2.0";
                case FrameworkVersion.FrameworkVersion35:
                    return "v3.5";
                default:
                    throw new ArgumentException(string.Format("Unknown framework Version: {0}", version));
            }
        }

        public string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
        {
            return FrameworkLocator.Instance[version].MSBuild.AssemblyPath;
        }
    }
}
