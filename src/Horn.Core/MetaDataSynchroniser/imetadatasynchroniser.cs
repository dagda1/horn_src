using System.IO;
using Horn.Core.PackageStructure;

namespace Horn.Core.Tree.MetaDataSynchroniser
{
    public interface IMetaDataSynchroniser
    {
        void SynchronisePackageTree(IPackageTree packageTree);
    }
}