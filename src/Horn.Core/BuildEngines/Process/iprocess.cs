using System.Diagnostics;

namespace Horn.Core.BuildEngines
{
    public interface IProcess
    {
        string GetLineOrOutput();
		bool IsComplete { get; }
        void WaitForExit();
    }
}