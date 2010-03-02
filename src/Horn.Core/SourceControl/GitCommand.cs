using Horn.Core.BuildEngines;

namespace Horn.Core.SCM
{
	public abstract class GitCommand : IGitCommand
	{
		public string Run(string command)
		{
			return GitCommands.GitCommands.RunCmd(Exe, Args(command));
		}

		public IProcess Run(string command, string workingDirectory)
		{
			return new DiagnosticsProcessFactory().GetProcess(Exe, Args(command), workingDirectory);
		}

		protected abstract string Args(string command);
		protected abstract string Exe { get; }
	}
}