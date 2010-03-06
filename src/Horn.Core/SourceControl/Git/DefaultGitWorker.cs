using System;
using System.IO;

using Horn.Core.BuildEngines;
using Horn.Core.exceptions;
using Horn.Core.Utils;

namespace Horn.Core.SCM
{
	public class DefaultGitWorker
		: GitWorkerBase
	{
		public DefaultGitWorker()
			: this(FindGitCmdDirectory())
		{
		}

		public DefaultGitWorker(string gitCmdDirectory)
		{
			_gitCmdDirectory = gitCmdDirectory;
		}

		private readonly string _gitCmdDirectory;
		public string GitCmdDirectory
		{
			get { return _gitCmdDirectory; }
		}

		protected override IProcess BuildGitCommandProcess(DirectoryInfo workingDirectory, string arguments)
		{
			string executable = Path.Combine(GitCmdDirectory, "git.cmd");
			if (!File.Exists(executable))
			{
				throw new FileNotFoundException("Could not find git.exe", executable);
			}

			IProcess result = new DiagnosticsProcessFactory().GetProcess(executable, arguments, workingDirectory.FullName);
			return result;
		}

		public static string FindGitCmdDirectory()
		{
			string cmdDirectory = new EnvironmentVariable().GetDirectoryFor("git.cmd");

			if (string.IsNullOrEmpty(cmdDirectory))
			{
				throw new EnvironmentVariableNotFoundException("No path to git was discovered. Either add the path to git.cmd to your PATH environment variable, or set the bashdirectory attribute in the Horn config file.");
			}

			return cmdDirectory;
		}
	}
}