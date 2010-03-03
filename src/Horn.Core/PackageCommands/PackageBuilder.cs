using System;

using Castle.Core;

using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.SCM;
using Horn.Core.Utils.CmdLine;

using log4net;

namespace Horn.Core.PackageCommands
{
    [Transient]
    public class PackageBuilder
        : PackageBuilderBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PackageBuilder));

        public override void Execute(PackageStructure.IPackageTree packageTree)
        {
            PackageEnvironmentInitialization.InitialiseForClearEnvironment();
            LogPackageDetails();

            base.Execute(packageTree);
        }

        protected virtual void LogPackageDetails()
        {
            var message = string.Format("installing {0} ", packageArgs.PackageName);

            if (!string.IsNullOrEmpty(packageArgs.Version))
                message += string.Format(" Version {0}", packageArgs.Version);

            if (!string.IsNullOrEmpty(packageArgs.Mode))
                message += string.Format(" Mode {0}.", packageArgs.Mode);

            log.Info(message + ".");
        }

        public PackageBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs, PackageArgs packageArgs)
            : base(get, processFactory, commandArgs, packageArgs)
        {
        }
    }
}
