using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dsl
{
    public interface IRepositoryElement
    {
        void Export();

        string ExportPath { get; }
        
        string IncludePath { get; }
        
        string RepositoryName { get; }
        
        IRepositoryElement PrepareRepository(IPackageTree packageToExportTo, IGet get);
    }
}