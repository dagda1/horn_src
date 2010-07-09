using Horn.Core.Config;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;

namespace Horn.Core.Tree.MetaDataSynchroniser
{
    public class MetaDataSynchroniser : IMetaDataSynchroniser
    {
        private readonly SourceControl sourceControl;

        public readonly static string PackageTreeUri = HornConfig.Settings.PackageTreeUri;

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