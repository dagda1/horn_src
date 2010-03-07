using System.Collections.Generic;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dependencies
{
    public interface IDependencyTree : IEnumerable<IPackageTree>
    {
        IList<IPackageTree> BuildList { get; }
    }
}