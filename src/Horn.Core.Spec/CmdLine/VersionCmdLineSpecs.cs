using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_a_version_switch_is_provided : CmdLineSpecificationBase 
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-version:2.1.0" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();
        }

        [Fact]
        public void Then_the_parsed_arguments_are_valid()
        {
            Assert.True(IsValid);
        }

        [Fact]
        public void Then_the_command_arguments_contain_the_version_number()
        {
            Assert.Equal("2.1.0", parser.CommandArguments.Packages[0].Version);
        }

        [Fact]
        public void Then_the_package_name_is_correct()
        {
            Assert.Equal("nhibernate", parser.CommandArguments.Packages[0].PackageName);
        }
    }

    public class When_a_version_switch_is_provided_with_no_argument : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-Version:2.1.0" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();
        }

        [Fact]
        public void Then_the_parsed_arguments_are_invalid()
        {
            Assert.True(IsValid);
        }
    }
}