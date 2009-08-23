namespace Horn.Core.Utils.CmdLine
{
    public interface ICommandArgs
    {
        string PackageName { get; }
        bool RebuildOnly { get; }
        string Version { get; }
        bool Refresh { get; }
        string OutputPath { get; }
    }
}