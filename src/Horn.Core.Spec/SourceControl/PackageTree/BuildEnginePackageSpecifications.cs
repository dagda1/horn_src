using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.helpers;
using Xunit;

namespace Horn.Core.Spec.Unit.HornTree
{
    public class When_the_nant_executable_is_required : Specification
    {
        private IPackageTree packageTree;
        private FileInfo executable;

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempPackageTree();
        }

        protected override void Because()
        {
            executable = packageTree.Nant;
        }

        [Fact]
        public void Then_the_nant_path_is_returned()
        {
            Assert.True(executable.Exists);
        }
    }
}