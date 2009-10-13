using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_a_mode_switch_is_provided : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-mode:debug" };

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
        public void Then_the_command_arguments_contain_the_mode()
        {
            Assert.Equal("debug", parser.CommandArguments.Mode);
        }
    }

    public class When_a_mode_switch_is_provided_with_no_argument : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate", "-mode" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();
        }

        [Fact]
        public void Then_the_parsed_arguments_are_not_valid()
        {
            Assert.False(IsValid);
        }
    }

    public class When_no_mode_switch_is_specified : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:nhibernate" };

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
        public void Then_the_mode_is_null()
        {
            Assert.Equal( null, parser.CommandArguments.Mode );
        }
    }
}
