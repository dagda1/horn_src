using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Framework.helpers;
using Horn.Spec.Framework.Stubs;

namespace Horn.Core.Spec.MetaSynchroniserfixture
{
    public abstract class MetaSynchroniserFixtureBase : Specification
    {

        protected IPackageTree packageTreeBase;
        protected IMetaDataSynchroniser metaDataSynchroniser;
        protected SourceControlDouble sourceControlDouble;


        protected override void Before_each_spec()
        {
            sourceControlDouble = new SourceControlDouble("http://www.someurlorsomething.com/");

            metaDataSynchroniser = new MetaDataSynchroniser(sourceControlDouble);

            packageTreeBase = new PackageTree(metaDataSynchroniser);
        }



    }
}