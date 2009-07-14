using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.helpers;
using log4net;
using Xunit;

namespace Horn.Core.Spec.PackageTreeSpecs
{
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