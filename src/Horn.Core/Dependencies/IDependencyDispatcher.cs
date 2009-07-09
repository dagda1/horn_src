using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dependencies
{
    public interface IDependencyDispatcher
    {
        void Dispatch(IPackageTree packageTree, IEnumerable<Dependency> dependencies, string dependenciesRoot);
    }
}