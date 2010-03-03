using System;
using Horn.Core.Utils.CmdLine;

namespace Horn.Spec.Framework.doubles
{
    public class CommandArgsDouble : ICommandArgs
    {
        private readonly PackageArgs singlePackage = new PackageArgs();
        public PackageArgs SinglePackage
        {
            get { return singlePackage; }
        }

        private readonly PackageArgs[] packages;
        public PackageArgs[] Packages
        {
            get { return packages; }
        }

        public bool RebuildOnly { get; private set; }
        public bool Refresh { get; set; }
        public string OutputPath { get; set; }

        public CommandArgsDouble(string installName)
        {
            packages = new[] { singlePackage };
            SinglePackage.PackageName = installName;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly) : this(installName)
        {
            RebuildOnly = rebuildOnly;
        }

        public CommandArgsDouble(string installName, string version)
            : this(installName)
        {
            SinglePackage.Version = version;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly, string version)
            : this(installName, rebuildOnly)
        {
            SinglePackage.Version = version;
        }

        public CommandArgsDouble(string installName, bool rebuildOnly, string version, string outputPath)
            :this(installName,rebuildOnly,version)
        {
            OutputPath = outputPath;
        }
    }
}