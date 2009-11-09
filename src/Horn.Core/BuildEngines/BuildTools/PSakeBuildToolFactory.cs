using System;

namespace Horn.Core
{
	public static class PSakeBuildToolFactory 
	{
		public static IBuildTool Create()
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEAMCITY_VERSION")))
				return new CmdHostedPSakeBuildTool();
			return new PowerShellHostedPSakeBuildTool();
		}
	}
}