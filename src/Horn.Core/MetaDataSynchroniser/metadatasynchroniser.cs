using Horn.Core.PackageStructure;
using Horn.Core.SCM;

namespace Horn.Core.Tree.MetaDataSynchroniser
{
    public class MetaDataSynchroniser : IMetaDataSynchroniser
    {
        private readonly SourceControl sourceControl;

        public const string PackageTreeUri = "git://github.com/dagda1/hornget.git";

        public void SynchronisePackageTree(IPackageTree packageTree)
        {
            sourceControl.RetrieveSource(packageTree);
        }

        public MetaDataSynchroniser(SourceControl sourceControl)
        {
            this.sourceControl = sourceControl;
        }
    }
}