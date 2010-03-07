namespace Horn.Core.BuildEngines
{
	public class CommandLineRunner : IShellRunner
	{
		private readonly IProcessFactory processFactory;

		public CommandLineRunner(IProcessFactory processFactory)
		{
			this.processFactory = processFactory;
		}

		public string RunCommand(string command, string args, string workingDirectory)
		{
			IProcess process = processFactory.GetProcess(command, args, workingDirectory);
			return process.GetLineOrOutput();
		}

		public string RunCommand(string command, string args)
		{
			return RunCommand(command, args, null);
		}
	}
}