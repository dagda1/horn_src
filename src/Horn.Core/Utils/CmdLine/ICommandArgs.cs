namespace Horn.Core.Utils.CmdLine
{
    public interface ICommandArgs
    {
        string OutputPath { get; }

        string Mode { get; }

        string PackageName { get; }

        bool RebuildOnly { get; }

        bool Refresh { get; }

        string Version { get; }               
    }
}