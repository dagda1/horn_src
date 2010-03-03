using System;

using Castle.Core;

using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

using log4net;

namespace Horn.Core.PackageCommands
{
    [Transient]
    public class PackagesBuilder
        : IPackageCommand
    {
        protected readonly IGet get;
        protected readonly IProcessFactory processFactory;
        protected readonly ICommandArgs commandArgs;
        private static readonly ILog log = LogManager.GetLogger(typeof(PackagesBuilder));

        public void Execute(IPackageTree packageTree)
        {
            PackageEnvironmentInitialization.InitialiseForClearEnvironment();

            foreach (var packageArgs in commandArgs.Packages)
            {
                PackageBuilderBase child = new PackageBuilderBase(get, processFactory, commandArgs, packageArgs);
                child.Execute(packageTree);
            }
        }

        protected virtual void LogPackageDetails()
        {
            foreach (var packageArgs in commandArgs.Packages)
            {
                var message = string.Format("installing {0} ", packageArgs.PackageName);

                if (!string.IsNullOrEmpty(packageArgs.Version))
                    message += string.Format(" Version {0}", packageArgs.Version);

                if (!string.IsNullOrEmpty(packageArgs.Mode))
                    message += string.Format(" Mode {0}.", packageArgs.Mode);

                log.Info(message + ".");
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