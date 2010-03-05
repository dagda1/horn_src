using System.Diagnostics;

namespace Horn.Core.BuildEngines
{
    public class DiagnosticsProcess : IProcess
    {
        private readonly Process process;

        public string GetLineOrOutput()
        {
            return process.StandardOutput.ReadLine() ?? process.StandardError.ReadLine();
        }

		public bool IsComplete
		{
			get { return process.HasExited; }
		}

        public void WaitForExit()
        {
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new ProcessFailedException(process.ExitCode);
        }

        public DiagnosticsProcess(Process process)
        {
            this.process = process;
        }
    }
}