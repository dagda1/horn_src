using System;
using System.IO;

using Horn.Core.BuildEngines;

namespace Horn.Core.SCM
{
	public class BashGitWorker
		: GitWorkerBase
	{
		public BashGitWorker(string bashDirectory)
		{
			_bashDirectory = bashDirectory;
		}

		private readonly string _bashDirectory;
		public string BashDirectory
		{
			get { return _bashDirectory; }
		}

		protected override IProcess BuildGitCommandProcess(DirectoryInfo workingDirectory, string arguments)
		{
			string executable = Path.Combine(BashDirectory, "bash.exe");
			if (!File.Exists(executable))
			{
				throw new FileNotFoundException("Could not find bash.exe", executable);
			}

			arguments = string.Format("--login -c 'git {0}'", arguments);

			IProcess result = new DiagnosticsProcessFactory().GetProcess(executable, arguments, Environment.CurrentDirectory);
			return result;
		}
	}
}