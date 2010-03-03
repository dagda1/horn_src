 using System;
 using System.IO;
 using Horn.Core.PackageStructure;
 using Horn.Core.Utils.CmdLine;
 using Horn.Framework.helpers;
 using Horn.Spec.Framework.helpers;
 using Xunit;

namespace Horn.Core.Spec.RevisionDataSpecs
{
    public class When_there_is_no_revision_data_for_a_versioned_package_request : Specification
    {
        private IPackageTree package;
        private IRevisionData revisionData;

        protected override void Before_each_spec()
        {
            var root = TreeHelper.GetTempPackageTree().RetrievePackage(PackageTreeHelper.PackageWithoutRevision);

            package = root.RetrievePackage(CommandLineHelper.GetCommandLineArgs("castle", "2.1.0").Packages[0]);
        }

        protected override void Because()
        {
            revisionData = new RevisionData(package);
        }

        [Fact]
        public void Then_a_versioned_revision_data_file_is_created()
        {
            var revisionDataFile = Path.Combine(package.CurrentDirectory.FullName,
                                            string.Format(RevisionData.VersionedFileName, "2.1.0"));

            Assert.True(revisionData.Revision.Length > 0);
        }

        [Fact]
        public void Then_the_revision_data_indicates_a_checkout_is_required()
        {
            Assert.True(revisionData.ShouldCheckOut());
        }
    }

    public class When_the_revision_data_for_a_versioned_package_is_requested : Specification
    {
        private IPackageTree package;
        private IRevisionData revisionData;

        protected override void Before_each_spec()
        {
            package = TreeHelper.GetTempPackageTree().RetrievePackage(CommandLineHelper.GetCommandLineArgs(PackageTreeHelper.PackageWithRevision, "2.1.0").Packages[0]);
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
}