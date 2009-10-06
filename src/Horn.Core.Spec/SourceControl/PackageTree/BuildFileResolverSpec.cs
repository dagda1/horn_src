using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.Unit.dsl;
using Xunit;

namespace Horn.Core.Spec.Unit.HornTree
{
    public class When_resolving_build_file : BaseDSLSpecification
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
        public void Then_the_version_number_is_parsed_from_the_file_name()
        {
            Assert.Equal("trunk", _fileResolver.Resolve(buildFolder, "horn").Version);
        }
    }
}