using System.IO;
using Horn.Core.Config;
using Horn.Core.exceptions;
using Horn.Core.Utils;

namespace Horn.Core.SCM
{
	public class GitBinDirectoryFinder
	{
		public string FindPreferred()
		{
			if (!string.IsNullOrEmpty(FindFromConfig())) return FindFromConfig();

			return FindFromEnvironmentVariable();
		}

		public string FindFromConfig()
		{
			return HornConfig.Settings.BashDirectory;
		}

		public string FindFromEnvironmentVariable()
		{
			var cmdDirectory = new EnvironmentVariable().GetDirectoryFor("git.cmd");

			if (string.IsNullOrEmpty(cmdDirectory))
				throw new EnvironmentVariableNotFoundException("No path to git was discovered. Either add the path to git.cmd to your PATH environment variable, or set the bashdirectory attribute in the Horn config file.");

			return Path.Combine(new DirectoryInfo(cmdDirectory).Parent.FullName, "bin");
		}
	}
}
