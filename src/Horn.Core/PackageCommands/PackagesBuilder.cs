using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Core;

using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
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

        private readonly List<IPackageTree> _alreadyBuilt = new List<IPackageTree>();
        public List<IPackageTree> AlreadyBuilt
        {
            get { return _alreadyBuilt; }
        }

        public void Execute(IPackageTree packageTree)
        {
            AlreadyBuilt.Clear();
            LogPackagesDetails();
            PackageEnvironmentInitialization.InitialiseForClearEnvironment();

            foreach (var packageArgs in commandArgs.Packages)
            {
                FilteredPackageBuilder child = new FilteredPackageBuilder(get, processFactory, commandArgs, packageArgs, AlreadyBuilt);
                child.Execute(packageTree);
            }
        }

        protected virtual void LogPackagesDetails()
        {
            StringBuilder details = new StringBuilder();
            foreach (PackageArgs packageArgs in commandArgs.Packages)
            {
                if (details.Length > 0)
                {
                    details.AppendLine();
                }

                details.AppendFormat("installing {0} ", packageArgs.PackageName);

                if (!string.IsNullOrEmpty(packageArgs.Version))
                {
                    details.AppendFormat(" Version {0}", packageArgs.Version);
                }

                if (!string.IsNullOrEmpty(packageArgs.Mode))
                {
                    details.AppendFormat(" Mode {0}.", packageArgs.Mode);
                }
            }

            log.Info(details);
        }

        public PackagesBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs)
        {
            this.get = get;
            this.processFactory = processFactory;
            this.commandArgs = commandArgs;
        }

        private class FilteredPackageBuilder
            : PackageBuilderBase
        {
            private readonly List<IPackageTree> alreadyBuilt;

            public FilteredPackageBuilder(IGet get, IProcessFactory processFactory, ICommandArgs commandArgs, PackageArgs packageArgs, List<IPackageTree> alreadyBuilt)
                : base(get, processFactory, commandArgs, packageArgs)
            {
                this.alreadyBuilt = alreadyBuilt;
            }

            protected override IDependencyTree GetDependencyTree(IPackageTree componentTree)
            {
                var sourceTree = base.GetDependencyTree(componentTree);
                return new AlreadyBuiltFilteredDependencyTree(sourceTree, alreadyBuilt);
            }
        }

        private class AlreadyBuiltFilteredDependencyTree
            : IDependencyTree
        {
            private readonly IList<IPackageTree> _packageTree;
            public IList<IPackageTree> BuildList
            {
                get { return _packageTree; }
            }

            public IEnumerator<IPackageTree> GetEnumerator()
            {
                return BuildList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public AlreadyBuiltFilteredDependencyTree(IDependencyTree source, List<IPackageTree> alreadyBuilt)
            {
                IList<IPackageTree> original = source.BuildList;
                IList<IPackageTree> filtered = original.Except(alreadyBuilt).ToList();
                _packageTree = filtered;

                Func<IList<IPackageTree>, string> toText = tree => tree.Count == 0 ? "-None-" : string.Join(", ", tree.Select(t => t.FullName).ToArray());

                log.InfoFormat("Requested package tree: {0}\nAlready built: {1}\nFiltered: {2}", toText(original), toText(alreadyBuilt), toText(filtered));

                alreadyBuilt.AddRange(filtered);
            }
        }
    }
}