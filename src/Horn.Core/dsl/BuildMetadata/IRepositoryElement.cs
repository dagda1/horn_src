using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dsl
{
    public interface IRepositoryElement
    {
        string ExportPath { get; }
        string IncludePath { get; }
        string RepositoryName { get; }
        void Export();
        IRepositoryElement PrepareRepository(IPackageTree packageToExportTo, IGet get);
    }
}