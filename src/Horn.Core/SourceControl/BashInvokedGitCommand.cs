using GitCommands;

namespace Horn.Core.SCM
{
	public class BashInvokedGitCommand : GitCommand
	{
		protected override string Args(string command)
		{
			return string.Format("--login -c \"git {0}\"", command);
		}

		protected override string Exe
		{
			get { return string.Format("{0}bash.exe", Settings.GitBinDir); }
		}
	}
}