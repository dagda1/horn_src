using System;
using System.Linq;
using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_multiple_values_are_comma_separated : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-installmultiple:first@version#mode,second#mode,third@version,fourth" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);
        }

        [Fact]
        public void Then_the_ParsedArgs_should_contain_an_array()
        {
            Assert.Contains("first@version#mode", parser.ParsedArgs["installmultiple"]);
            Assert.Contains("second#mode", parser.ParsedArgs["installmultiple"]);
            Assert.Contains("third@version", parser.ParsedArgs["installmultiple"]);
            Assert.Contains("fourth", parser.ParsedArgs["installmultiple"]);
        }
    }

    public class When_an_installmultiple_switch_is_provided : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-installmultiple:first@version1#mode1,second#mode2,third@version3,fourth" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();
        }

        [Fact]
        public void Then_the_parsed_arguments_are_valid()
        {
            AssertIsValid();
        }

        [Fact]
        public void Then_the_package_arguments_contain_the_first_package()
        {
            PackageArgs package = parser.CommandArguments.Packages.First(pkg => pkg.PackageName == "first");
            Assert.Equal("version1", package.Version);
            Assert.Equal("mode1", package.Mode);
        }

        [Fact]
        public void Then_the_package_arguments_contain_the_second_package()
        {
            PackageArgs package = parser.CommandArguments.Packages.First(pkg => pkg.PackageName == "second");
            Assert.Null(package.Version);
            Assert.Equal("mode2", package.Mode);
        }

        [Fact]
        public void Then_the_package_arguments_contain_the_third_package()
        {
            PackageArgs package = parser.CommandArguments.Packages.First(pkg => pkg.PackageName == "third");
            Assert.Equal("version3", package.Version);
            Assert.Null(package.Mode);
        }

        [Fact]
        public void Then_the_package_arguments_contain_the_fourth_package()
        {
            PackageArgs package = parser.CommandArguments.Packages.First(pkg => pkg.PackageName == "fourth");
            Assert.Null(package.Version);
            Assert.Null(package.Mode);
        }
    }
}