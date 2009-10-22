using System;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Spec.Framework;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using NUnit.Framework;
using Xunit;

namespace Horn.Core.Spec.SCM
{
    public class A_When_the_source_does_not_exist_on_the_client : Specification 
    {
        private GitSourceControlDouble gitSourceControl;

        private IPackageTree packageTree;

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempEmptyPackageTree();

            gitSourceControl = new GitSourceControlDouble(MetaDataSynchroniser.PackageTreeUri, new EnvironmentVariable());
        }

        protected override void Because()
        {
            gitSourceControl.RetrieveSource(packageTree);
        }

        [Fact]
        public void Then_the_source_is_retrieved()
        {            
        }
    }
}