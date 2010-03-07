using Horn.Core.PackageStructure;
using Horn.Core.SCM;

namespace Horn.Core.GetOperations
{
    public interface IGet
    {
        IGet From(SourceControl sourceControlToGetFrom);
        
        IPackageTree ExportTo(IPackageTree packageTree);

        IPackageTree ExportTo(IPackageTree packageTree, string path, bool initialise);
    }
}