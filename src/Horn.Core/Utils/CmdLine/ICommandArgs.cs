namespace Horn.Core.Utils.CmdLine
{
    public interface ICommandArgs
    {
        string OutputPath { get; }

        PackageArgs[] Packages { get; }

        bool RebuildOnly { get; }

        bool Refresh { get; }
    }
}