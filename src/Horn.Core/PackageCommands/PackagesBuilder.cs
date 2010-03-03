using System;

using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

namespace Horn.Core.PackageCommands
{
    public class PackagesBuilder
        : IPackageCommand
    {
        protected readonly IGet get;
        protected readonly IProcessFactory processFactory;
        protected readonly ICommandArgs commandArgs;

        public void Execute(IPackageTree packageTree)
        {
            foreach (var packageArgs in commandArgs.Packages)
            {
                PackageBuilder child = new PackageBuilder(get, processFactory, commandArgs, packageArgs);
                child.Execute(packageTree);
            }
        }

        public PackagesBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs)
        {
            this.get = get;
            this.processFactory = processFactory;
            this.commandArgs = commandArgs;
        }
    }
}