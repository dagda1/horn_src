using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_a_output_switch_is_provided : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-output:c:\temp" };

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
        public void Then_the_command_arguments_contain_the_output_number()
        {
            Assert.Equal("c:\temp", parser.CommandArguments.OutputPath);
        }

        [Fact]
        public void Then_the_package_name_is_correct()
        {
            Assert.Equal("nhibernate", parser.CommandArguments.PackageName);
        }
    }

    public class When_a_output_switch_is_provided_with_no_argument : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-output" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();
        }

        [Fact]
        public void Then_the_parsed_arguments_are_invalid()
        {
            Assert.False(IsValid);
        }
    }
}
