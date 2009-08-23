using System;
using Horn.Core.Utils.CmdLine;

namespace Horn.Spec.Framework.doubles
{
    public class CommandArgsDouble : ICommandArgs
    {
        public string PackageName{ get; private set; }

        public bool RebuildOnly { get; private set; }
        public string Version { get; private set; }
        public bool Refresh { get; set; }
        public string OutputPath { get; set; }

        public CommandArgsDouble(string installName)
        {
            PackageName = installName;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly) : this(installName)
        {
            PackageName = installName;
            RebuildOnly = rebuildOnly;
        }

        public CommandArgsDouble(string installName, string version)
            : this(installName)
        {
            PackageName = installName;
            Version = version;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly, string version)
            : this(installName, rebuildOnly)
        {
            Version = version;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly, string version, string outputPath)
            :this(installName,rebuildOnly,version)
        {
            OutputPath = outputPath;
        }
    }
}