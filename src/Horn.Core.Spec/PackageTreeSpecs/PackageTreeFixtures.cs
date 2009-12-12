using System.IO;
using System.Linq;
using Horn.Core.PackageStructure;
using Horn.Spec.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.PackageTreeSpecs
{
    public class When_the_package_tree_has_been_created : Specification
    {
        private IPackageTree fakeTree;

        protected override void Because()
        {
            fakeTree = TreeHelper.GetTempPackageTree();
        }

        [Fact]
        public void Then_only_nodes_with_boo_files_are_added()
        {
            Assert.True(fakeTree.BuildNodes()[0].IsBuildNode);
        }
    }

    public class When_we_need_a_default_directory_for_the_output_of_the_build : Specification
    {
        private IPackageTree packageTree;

        protected override void Because()
        {
            packageTree = TreeHelper.GetTempPackageTree();
        }

        [Fact]
        public void Then_an_output_directory_should_exist_in_the_root_directory()
        {
            Assert.True(Directory.Exists(packageTree.Result.FullName));
        }
    }
}