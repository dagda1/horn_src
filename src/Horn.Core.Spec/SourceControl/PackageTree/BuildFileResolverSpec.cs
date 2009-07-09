using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.Unit.dsl;
using Xunit;

namespace Horn.Core.Spec.Unit.HornTree
{
    public class When_resolving_a_boo_build_file : BaseDSLSpecification
    {
        private IBuildFileResolver _fileResolver;
        private DirectoryInfo buildFolder;

        protected override void Before_each_spec()
        {
            _fileResolver = new BuildFileResolver();
        }

        protected override void Because()
        {
            buildFolder = GetTestBuildConfigsFolder();
        }

        [Fact]
        public void Then_a_boo_extension_is_returned_from_the_resolver()
        {
            Assert.Equal("boo", _fileResolver.Resolve(buildFolder, "horn").Extension);
        }
    }
}