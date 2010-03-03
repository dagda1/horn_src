using System;
using System.Collections;
using System.Collections.Generic;

using Castle.Core;

using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;

using log4net;

using System.Linq;

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
            PackageEnvironmentInitialization.InitialiseForClearEnvironment();

            foreach (var packageArgs in commandArgs.Packages)
            {
                FilteredPackageBuilder child = new FilteredPackageBuilder(get, processFactory, commandArgs, packageArgs, AlreadyBuilt);
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

        public class AlreadyBuiltFilteredDependencyTree
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

                log.DebugFormat("Requested package tree: {0}\nAlready built: {1}\nFiltered: {2}", toText(original), toText(alreadyBuilt), toText(filtered));

                alreadyBuilt.AddRange(filtered);
            }
        }

        public class FilteredPackageBuilder
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
    }
}