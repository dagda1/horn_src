using System;
using System.Collections.Generic;
using System.Text;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
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
					 "{0} /p:OutputPath=\"{1}\"  /p:TargetFrameworkVersion={2} /p:NoWarn=1591 /consoleloggerparameters:Summary",
					 pathToBuildFile.QuotePath(), Path.Combine(packageTree.WorkingDirectory.FullName, buildEngine.BuildRootDirectory),
					 GetFrameworkVersionForBuildTool(version));

			AppendTasks(buildEngine, cmdLine);
			AppendParameters(buildEngine, cmdLine);

			return cmdLine.ToString();
		}

    	private static void AppendParameters(BuildEngine buildEngine, StringBuilder cmdLine)
    	{
    		if (buildEngine.Parameters == null || buildEngine.Parameters.Count == 0)
				return;

    		foreach (var parameter in buildEngine.Parameters)
    		{
    			cmdLine.AppendFormat(" {0}={1}", parameter.Key, parameter.Value);
    		}
    	}

    	private static void AppendTasks(BuildEngine buildEngine, StringBuilder cmdLine)
    	{
    		if (buildEngine.Tasks == null || buildEngine.Tasks.Count == 0)
    			return;

    	    var tasks = new List<string>( buildEngine.Tasks );
    		cmdLine.AppendFormat(" /t:{0}", String.Join(";", tasks.ToArray()));
    	}

    	public string GetFrameworkVersionForBuildTool(FrameworkVersion version)
        {
            switch (version)
            {
                case FrameworkVersion.FrameworkVersion2:
                    return "v2.0";
                case FrameworkVersion.FrameworkVersion35:
                    return "v3.5";
                case FrameworkVersion.FrameworkVersion40:
                    return "v4.0";
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
