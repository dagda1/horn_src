using System;
using Horn.Core.BuildEngines;

namespace Horn.Spec.Framework.Stubs
{
    public class StubProcessFactory : IProcessFactory 
    {
        public void ExcuteCommand(string command, string workingDirectory)
        {
            Console.WriteLine(command);

            Console.WriteLine(workingDirectory);
        }

        public IProcess GetProcess(string pathToBuildTool, string cmdLineArguments, string workingDirectoryPath)
        {
            return new StubProcess();
        }
    }
}