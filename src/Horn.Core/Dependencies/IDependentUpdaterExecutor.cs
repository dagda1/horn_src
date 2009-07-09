using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dependencies
{
    public interface IDependentUpdaterExecutor
    {
        void Execute(IPackageTree packageTree, IEnumerable<string> dependencyPaths, Dependency dependency);
    }
}