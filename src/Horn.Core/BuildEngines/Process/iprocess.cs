using System.Diagnostics;

namespace Horn.Core.BuildEngines
{
    public interface IProcess
    {
        string GetLineOrOutput();

        void WaitForExit();
    }
}