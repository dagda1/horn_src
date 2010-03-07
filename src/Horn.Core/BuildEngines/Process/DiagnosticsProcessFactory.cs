using System;
using System.Diagnostics;

namespace Horn.Core.BuildEngines
{
    public class DiagnosticsProcessFactory : IProcessFactory
    {
        public void ExcuteCommand(string command, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            var process = Process.Start(processStartInfo);

            var streamWriter = process.StandardInput;

            var streamReader = process.StandardOutput;

            streamWriter.WriteLine(command);

            streamWriter.Close();

            process.WaitForExit();
        }

        public IProcess GetProcess(string pathToBuildTool, string cmdLineArguments, string workingDirectoryPath)
        {
            var psi = new ProcessStartInfo(pathToBuildTool, cmdLineArguments)
                          {
                              UseShellExecute = false,                      
                              RedirectStandardOutput = true,                      
                              RedirectStandardError = true,                      
                              WorkingDirectory = workingDirectoryPath,                      
                              Arguments = cmdLineArguments
                          };    
            
            return new DiagnosticsProcess(Process.Start(psi));
        }
    }
}
