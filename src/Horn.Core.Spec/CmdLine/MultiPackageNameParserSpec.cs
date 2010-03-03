using System;

using Horn.Core.Utils.CmdLine;

using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public abstract class MultiPackageNameParserSpecificationBase
        : Specification
    {
        protected readonly MultiPackageNameParser parser = new MultiPackageNameParser();
        protected PackageArgs package;
        protected Exception parseException;

        protected abstract string Arguments { get; }

        protected override void Because()
        {
            try
            {
                package = parser.Parse(Arguments);
            }
            catch (Exception ex)
            {
                parseException = ex;
            }
        }
    }

    public class When_only_a_package_name_is_provided
        : MultiPackageNameParserSpecificationBase
    {
        protected override string Arguments
        {
            get { return "packageName"; }
        }

        [Fact]
        public void Then_the_PackageName_is_set()
        {
            Assert.Equal("packageName", package.PackageName);
        }

        [Fact]
        public void Then_the_Version_is_null()
        {
            Assert.Null(package.Version);
        }

        [Fact]
        public void Then_the_Mode_is_null()
        {
            Assert.Null(package.Mode);
        }
    }

    public class When_a_package_name_and_version_are_provided
        : MultiPackageNameParserSpecificationBase
    {
        protected override string Arguments
        {
            get { return "packageName@version"; }
        }

        [Fact]
        public void Then_the_PackageName_is_set()
        {
            Assert.Equal("packageName", package.PackageName);
        }

        [Fact]
        public void Then_the_Version_is_set()
        {
            Assert.Equal("version", package.Version);
        }

        [Fact]
        public void Then_the_Mode_is_null()
        {
            Assert.Null(package.Mode);
        }
    }

    public class When_a_package_name_and_version_and_mode_are_provided
        : MultiPackageNameParserSpecificationBase
    {
        protected override string Arguments
        {
            get { return "packageName@version#mode"; }
        }

        [Fact]
        public void Then_the_PackageName_is_set()
        {
            Assert.Equal("packageName", package.PackageName);
        }

        [Fact]
        public void Then_the_Version_is_set()
        {
            Assert.Equal("version", package.Version);
        }

        [Fact]
        public void Then_the_Mode_is_set()
        {
            Assert.Equal("mode", package.Mode);
        }
    }
}