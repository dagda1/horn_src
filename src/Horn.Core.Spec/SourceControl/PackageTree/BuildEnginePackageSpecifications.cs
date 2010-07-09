using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using Horn.Spec.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.Unit.HornTree
{
    public class When_the_nant_executable_is_required : Specification
    {
        private IPackageTree packageTree;
        private NAntBuildTool nAntBuildTool;
        private FileInfo executable;

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempPackageTree();

            nAntBuildTool = new NAntBuildTool();
        }

        protected override void Because()
        {
            executable = new FileInfo(nAntBuildTool.PathToBuildTool(packageTree, FrameworkVersion.FrameworkVersion35));
        }

        [Fact]
        public void Then_the_nant_path_is_returned()
        {
            Assert.True(executable.Exists);
        }
    }
}