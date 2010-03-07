using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Xunit;

namespace Horn.Core.Spec.BuildEngineSpecs
{
    public class When_the_we_need_to_find_the_build_directory : Specification
    {
        private IPackageTree packageTree;

        private DirectoryInfo expected;
        private DirectoryInfo actual;
        private DirectoryInfo working;

        protected override void Before_each_spec()
        {
            packageTree = TreeHelper.GetTempPackageTree();   
        }

        protected override void Because()
        {
            var buildEngine = new BuildEngineStub(new BuildToolStub(), null, null);

            buildEngine.BuildRootDirectory = "build";

            working = packageTree.RetrievePackage("castle").WorkingDirectory;

            var buildRoot = new DirectoryInfo(Path.Combine(working.FullName,
                                                           buildEngine.BuildRootDirectory));

            actual = buildEngine.GetBuildDirectory(buildRoot);

            expected = new DirectoryInfo(Path.Combine(working.FullName, @"build\net-3.5\debug"));
        }

        [Fact]
        public void Then_horn_will_search_from_the_build_root_directory()
        {
            Assert.Equal(actual.FullName.ToLower(), expected.FullName.ToLower());
        }
    }

    public class When_the_build_directory_contains_no_build_files : Specification
    {
        private IPackageTree packageTree;

        private DirectoryInfo working;
        private DirectoryInfo buildRoot;
        private BuildEngineStub buildEngine;

        protected override void Because()
        {
            packageTree = TreeHelper.GetTempPackageTree();

            buildEngine = new BuildEngineStub(new BuildToolStub(), null, null);

            buildEngine.BuildRootDirectory = "build";

            working = packageTree.RetrievePackage("log4net").WorkingDirectory;

            buildRoot = new DirectoryInfo(Path.Combine(working.FullName,
                                                        buildEngine.BuildRootDirectory));
        }

        [Fact]
        public void Then_a_directory_not_found_exception_should_be_thrown()
        {
            Assert.Throws<DirectoryNotFoundException>(() => buildEngine.GetBuildDirectory(buildRoot));   
        }
    }
}