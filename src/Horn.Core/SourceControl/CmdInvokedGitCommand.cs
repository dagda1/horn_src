using GitCommands;

namespace Horn.Core.SCM
{
	public class CmdInvokedGitCommand : GitCommand
	{
		protected override string Args(string command)
		{
			return command;
		}

		protected override string Exe
		{
			get { return string.Format("{0}git.exe", Settings.GitBinDir); }
		}
	}
}