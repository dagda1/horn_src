using System;
using System.Diagnostics;
using System.Linq;
using Castle.Core;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.Extensions;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils.CmdLine;
using log4net;

namespace Horn.Core.PackageCommands
{
    [Transient]
    public class PackageBuilder : IPackageCommand
    {
        private readonly IGet get;
        private readonly IProcessFactory processFactory;
        private readonly ICommandArgs commandArgs;
        private readonly PackageArgs packageArgs;
        private static readonly ILog log = LogManager.GetLogger(typeof(PackageBuilder));

        public virtual void Execute(IPackageTree packageTree)
        {
            Initialise();

            LogPackageDetails();

            if (!packageTree.BuildNodes().Select(x => x.Name).ToList().Contains(packageArgs.PackageName))
                throw new UnknownInstallPackageException(string.Format("No package definition exists for {0}.", packageArgs.PackageName));
            
            IPackageTree componentTree = packageTree.RetrievePackage(commandArgs);

            IDependencyTree dependencyTree = GetDependencyTree(componentTree);

            BuildDependencyTree(packageTree, dependencyTree);

            log.InfoFormat("\nHorn has finished installing {0}.\n\n".ToUpper(), packageArgs.PackageName);
        }

        protected virtual void BuildDependencyTree(IPackageTree packageTree, IDependencyTree dependencyTree)
        {
            foreach (var nextTree in dependencyTree)
            {
                IBuildMetaData nextMetaData = GetBuildMetaData(nextTree);

                if (!commandArgs.RebuildOnly)
                    RetrieveSourceCode(nextMetaData, nextTree);

                ExecutePrebuildCommands(nextMetaData, nextTree);

                BuildSource(nextTree, nextMetaData);
            }
        }

        protected virtual void BuildSource(IPackageTree nextTree, IBuildMetaData nextMetaData)
        {
            log.InfoFormat("\nHorn is building {0}.\n\n".ToUpper(), nextMetaData.InstallName);

            nextMetaData.BuildEngine.Build(processFactory, nextTree, packageArgs.Mode);
        }

        protected virtual void ExecutePrebuildCommands(IBuildMetaData metaData, IPackageTree packageTree)
        {
            packageTree.PatchPackage();

            if (!metaData.PrebuildCommandList.HasElements())
                return;

            foreach (var command in metaData.PrebuildCommandList)
            {
                processFactory.ExcuteCommand(command, packageTree.WorkingDirectory.FullName);
            }
        }

        protected virtual IBuildMetaData GetBuildMetaData(IPackageTree nextTree)
        {
            return nextTree.GetBuildMetaData(nextTree.BuildFile);
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

        protected virtual IDependencyTree GetDependencyTree(IPackageTree componentTree)
        {
            return new DependencyTree(componentTree);
        }

        protected virtual void RetrieveSourceCode(IBuildMetaData buildMetaData, IPackageTree componentTree)
        {
            ExecuteRepositoryElementList(buildMetaData, componentTree);

            ExecuteExportList(buildMetaData, componentTree);

            if (buildMetaData.SourceControl == null)
                return;

            log.InfoFormat("\nHorn is fetching {0}.\n\n".ToUpper(), buildMetaData.SourceControl.Url);

            get.From(buildMetaData.SourceControl).ExportTo(componentTree);
        }

        protected virtual void ExecuteExportList(IBuildMetaData buildMetaData, IPackageTree componentTree)
        {
            if (!buildMetaData.ExportList.HasElements())
                return;

            var initialise = true;

            foreach (var sourceControl in buildMetaData.ExportList)
            {
                log.InfoFormat("\nHorn is fetching {0}.\n\n".ToUpper(), sourceControl.Url);

                get.From(sourceControl).ExportTo(componentTree, sourceControl.ExportPath, initialise);

                initialise = false;
            }
        }

        protected virtual void ExecuteRepositoryElementList(IBuildMetaData buildMetaData, IPackageTree componentTree)
        {
            if (!buildMetaData.RepositoryElementList.HasElements())
                return;

            foreach (var repositoryElement in buildMetaData.RepositoryElementList)
            {
                repositoryElement.PrepareRepository(componentTree, get).Export();
            }
        }

        protected virtual void Initialise()
        {
            BuildEngine.ClearBuiltPackages();

            SourceControl.ClearDownLoadedPackages();
        }

        public PackageBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs)
        {
            this.get = get;
            this.processFactory = processFactory;
            this.commandArgs = commandArgs;
            packageArgs = commandArgs.Packages[0];
        }
    }
}
