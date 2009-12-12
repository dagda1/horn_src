namespace Horn.Core.BuildEngines
{
    public interface IProcessFactory
    {
        IProcess GetProcess(string pathToBuildTool, string cmdLineArguments, string workingDirectoryPath);

        void ExcuteCommand(string command, string workingDirectory);
    }
}