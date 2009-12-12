using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;
using Horn.Spec.Framework.Stubs;

namespace Horn.Core.Spec.Unit.GetSpecs
{
    using Utils;

    public abstract class GetSpecificationBase : DirectorySpecificationBase
    {
        protected IGet get;
        protected IFileSystemProvider fileSystemProvider;
        protected SourceControlDouble sourceControl;
        protected IBuildMetaData buildMetaData;
        protected IPackageTree packageTree;

        protected override void Before_each_spec()
        {   
            base.Before_each_spec();

            sourceControl = new SourceControlDouble("http://localhost/horn");

            packageTree = new PackageTree(rootDirectory, null);

            fileSystemProvider = CreateStub<IFileSystemProvider>();
        }
    }
}