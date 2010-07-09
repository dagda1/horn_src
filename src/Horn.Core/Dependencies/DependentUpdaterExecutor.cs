using System.Collections.Generic;
using System.Linq;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;

namespace Horn.Core.Dependencies
{
    public class DependentUpdaterExecutor :  WithLogging, IDependentUpdaterExecutor
    {
        private readonly HashSet<IDependentUpdater> updaters;

        public void Execute(IPackageTree packageTree, IEnumerable<string> dependencyPaths, Dependency dependency)
        {
            if (!HasADependencyToUpdate(dependencyPaths))
                return;

            InfoFormat("Dependency: Executing Dependency Updaters for {0}", packageTree.Name);

            var dependentUpdaterContext = new DependentUpdaterContext(packageTree, dependencyPaths, dependency);
            updaters.ForEach(updater => updater.Update(dependentUpdaterContext));
        }

        private bool HasADependencyToUpdate(IEnumerable<string> dependencyPaths)
        {
            return dependencyPaths != null && dependencyPaths.Count() > 0;
        }

        public DependentUpdaterExecutor(IEnumerable<IDependentUpdater> updaters)
        {
            this.updaters = new HashSet<IDependentUpdater>(updaters);
        }
    }
}