using System;
using System.Linq;

using Castle.Core;

using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.Extensions;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

using log4net;

namespace Horn.Core.PackageCommands
{
    [Transient]
    public class PackageBuilderBase : IPackageCommand
    {
        protected readonly IGet get;
        protected readonly IProcessFactory processFactory;
        protected readonly ICommandArgs commandArgs;
        protected readonly PackageArgs packageArgs;
        private static readonly ILog log = LogManager.GetLogger(typeof(PackageBuilderBase));

        public virtual void Execute(IPackageTree packageTree)
        {
            if (!packageTree.BuildNodes().Select(x => x.Name).ToList().Contains(packageArgs.PackageName))
                throw new UnknownInstallPackageException(string.Format("No package definition exists for {0}.", packageArgs.PackageName));
            
            IPackageTree componentTree = packageTree.RetrievePackage(packageArgs);

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

        public PackageBuilderBase(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs, PackageArgs packageArgs)
        {
            this.get = get;
            this.processFactory = processFactory;
            this.commandArgs = commandArgs;
            this.packageArgs = packageArgs;
        }   
    }
}