using System.IO;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dsl
{
    public interface IBuildConfigReader
    {
        IBuildMetaData GetBuildMetaData(string packageName);
        IBuildMetaData GetBuildMetaData(IPackageTree packageTree, string buildFile);
        IBuildConfigReader SetDslFactory(IPackageTree packageTree);
    }
}