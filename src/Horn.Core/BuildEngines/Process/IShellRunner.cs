using System;

namespace Horn.Core.BuildEngines
{
	public interface IShellRunner
	{
		string RunCommand(string command, string args, string workingDirectory);
		string RunCommand(string command, string args);
	}
}