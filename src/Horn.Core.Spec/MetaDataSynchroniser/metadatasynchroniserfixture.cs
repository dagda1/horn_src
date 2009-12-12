using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Spec.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.MetaSynchroniserfixture
{
    public class When_the_package_tree_structure_does_not_exist : MetaSynchroniserFixtureBase
    {
        private IPackageTree packageTree;

        protected override void Because()
        {
            SourceControl.ClearDownLoadedPackages();

            packageTree = packageTreeBase.GetRootPackageTree(TreeHelper.GetTempEmptyPackageTree().Root.CurrentDirectory);
        }

        [Fact]
        public void Then_horn_creates_the_root_folder()
        {
            Assert.True(packageTree.CurrentDirectory.Exists);
        }

        [Fact]
        public void Then_the_package_tree_contains_more_than_one_build_file()
        {
            var files = packageTree.CurrentDirectory.GetFiles("horn.*", SearchOption.AllDirectories);

            Assert.True(files.Length > 0);
        }
    }
}