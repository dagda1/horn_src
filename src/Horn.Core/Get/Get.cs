using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils;

namespace Horn.Core.GetOperations
{
    public class Get : IGet
    {
        protected readonly IFileSystemProvider fileSystemProvider;
        protected Package package;
        protected SourceControl sourceControl;

        public virtual IPackageTree ExportTo(IPackageTree packageTree)
        {
            sourceControl.RetrieveSource(packageTree);

            return packageTree;
        }

        public IPackageTree ExportTo(IPackageTree packageTree, string path, bool initialise)
        {
            sourceControl.RetrieveSource(packageTree, path, initialise);

            return packageTree;
        }

        public virtual IGet From(SourceControl sourceControlToGetFrom)
        {
            sourceControl = sourceControlToGetFrom;

            return this;
        }

        public virtual IGet Package(Package packageToGet)
        {
            package = packageToGet; 
            return this;
        }

        public Get(IFileSystemProvider fileSystemProvider)
        {
            this.fileSystemProvider = fileSystemProvider;
        }
    }
}