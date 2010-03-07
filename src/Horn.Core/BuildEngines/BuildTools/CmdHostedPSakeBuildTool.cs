using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;

namespace Horn.Core
{
	/// <summary>
	/// This <see cref="IBuildTool"/> implementation is used while horn runs on a TeamCity environment
	/// as a workaround for the following issue: http://jetbrains.net/tracker/issue/TW-6021.
	/// </summary>
	public class CmdHostedPSakeBuildTool : PSakeBuildToolBase
	{
		public override string CommandLineArguments(string pathToBuildFile, BuildEngine buildEngine, IPackageTree packageTree, FrameworkVersion version)
		{
			return string.Format(@"/c echo antani | powershell.exe -command {0}", GeneratePSakeCommand(pathToBuildFile, buildEngine));			
		}

		public override string PathToBuildTool(IPackageTree packageTree, FrameworkVersion version)
		{
			return "cmd.exe";
		}     
	}
}