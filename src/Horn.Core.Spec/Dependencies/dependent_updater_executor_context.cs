namespace Horn.Core.Spec.Dependencies
{
    using System.Collections.Generic;
    using BuildEngines;
    using Core.Dependencies;
    using PackageStructure;

    public abstract class dependent_updater_executor_context : Specification
    {
        protected DependentUpdaterExecutor executor;
        protected IDependentUpdater updater;
        protected Dependency dependency;
        protected IEnumerable<string> dependencyPaths;
        protected IPackageTree packageTree;
    }
}