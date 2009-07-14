using Horn.Core.PackageStructure;
using Horn.Core.Spec.helpers;
using Horn.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.RevisionDataSpecs
{
    public class When_a_package_has_no_revision_data : Specification
    {
        private IPackageTree package;
        private IRevisionData revisionData;

        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PackageWithoutRevision);
        }

        protected override void Because()
        {
            revisionData = new RevisionData(package);
        }

        [Fact]
        public void Then_a_new_revision_data_is_created()
        {
            Assert.Equal("0", revisionData.Revision);
        }

        [Fact]
        public void Then_the_revision_data_indicates_a_checkout_is_required()
        {
            Assert.True(revisionData.ShouldCheckOut());
        }
    }

    public class When_the_revision_data_for_a_package_is_requested : Specification
    {
        private IPackageTree package;
        private IRevisionData revisionData;

        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PackageWithRevision);
        }

        protected override void Because()
        {
            revisionData = new RevisionData(package);
        }

        [Fact]
        public void Then_the_revision_data_is_parsed_from_the_file()
        {
            Assert.Equal("1", revisionData.Revision);
        }
    }

    public class When_comparing_a_scm_revison_against_a_package_with_revision_data : Specification
    {
        private IPackageTree package;
        private IRevisionData treeRevisionData;
        private IRevisionData scmRevisionData;

        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PackageWithoutRevision);
        }

        protected override void Because()
        {
            treeRevisionData = new RevisionData(package);

            scmRevisionData = new RevisionData("1");
        }

        [Fact]
        public void Then_the_revision_data_indicates_an_update_is_required()
        {
            Assert.True(treeRevisionData.ShouldUpdate(scmRevisionData));
        }

        [Fact]
        public void Then_the_revision_data_indicates_a_checkout_is_not_required()
        {
            Assert.False(scmRevisionData.ShouldCheckOut());
        }
    }
}