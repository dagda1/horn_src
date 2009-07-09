using System;
using Horn.Core.BuildEngines;

namespace Horn.Core.Spec.BuildEngineSpecs
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