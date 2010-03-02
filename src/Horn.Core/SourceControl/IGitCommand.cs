using Horn.Core.BuildEngines;

namespace Horn.Core.SCM
{
	public interface IGitCommand
	{
		string Run(string command);
		IProcess Run(string command, string workingDirectory);
	}
}