using System;
using System.Diagnostics;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Framework.helpers;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Xunit;

namespace Horn.Core.Spec.SCM
{
    public class When_the_source_does_not_exist_on_the_client : Specification 
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
            Assert.NotEqual(gitSourceControl.Revision, Guid.Empty.ToString());
        }
    }

    public class When_the_source_does_exist_on_the_client : Specification
    {
        private GitSourceControlDouble gitSourceControl;

        private IPackageTree packageTree;

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PackageWithRevision);

            gitSourceControl = new GitSourceControlDouble(MetaDataSynchroniser.PackageTreeUri, new EnvironmentVariable());
        }

        protected override void Because()
        {
            gitSourceControl.RetrieveSource(packageTree);
        }

        [Fact]
        public void Then_the_source_is_retrieved()
        {
            Assert.False(string.IsNullOrEmpty(gitSourceControl.Revision));
        }
    }
}