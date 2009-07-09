using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_horn_recevies_an_Install_Switch_From_The_Command_Line : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] {"-install:horn"};
        private const string installName = "horn";

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
        public void Then_the_parsed_arguments_contain_the_install_name()
        {
            Assert.Equal(installName, parser.CommandArguments.PackageName);
        }

    }

    public class When_Horn_Receives_The_Help_Switch : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] {"-help"};

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);
        }


        [Fact]
        public void Then_Console_Should_Output_Help_Text()
        {
            AssertOutputContains(SwitchParser.HelpText);
        }

        [Fact]
        public void Then_A_Help_Return_Value_Is_returned()
        {
            Assert.IsAssignableFrom<HelpReturnValue>(parser.ParsedArgs);
        }

    }


    public class When_Horn_Receives_No_Command_Line_Arguments : CmdLineErrorSpecificationBase
    {
        protected override string[] Args
        {
            get { return null; }
        }

        protected override string ExpectErrorMessage
        {
            get { return "Missing required argument key"; }
        }

        [Fact]
        public void Then_Parsed_Arguments_Are_Not_Valid()
        {
            Assert.False(IsValid);
        }

        [Fact]
        public void Then_Horn_Outputs_A_Missing_argument_Error_Message()
        {
            AssertOutputContains(ExpectErrorMessage);
        }

    }

    public class When_Horn_Receives_No_Install_Argument : CmdLineErrorSpecificationBase
    {
        protected override string[] Args
        {
            get { return new[]{"-somearg:something"}; }
        }

        protected override string ExpectErrorMessage
        {
            get { return "Missing required argument key"; }
        }

        [Fact]
        public void Then_Parsed_Arguments_Are_Not_Valid()
        {
            Assert.False(IsValid);
        }

        [Fact]
        public void Then_Should_Output_Missing_argument_Error_Message()
        {
            AssertOutputContains(ExpectErrorMessage);
        }
    }


    public class When_Horn_Recevies_Install_Argument_With_No_Value : CmdLineErrorSpecificationBase
    {
        protected override string[] Args
        {
            get { return new[]{"-install:"}; }
        }

        protected override string ExpectErrorMessage
        {
            get { return "Missing argument value for key: install."; }
        }

        [Fact]
        public void Then_Parsed_Arguments_Are_Not_Valid()
        {
            Assert.False(IsValid);
        }

        [Fact]
        public void Then_Should_Output_Argument_Has_Already_Been_Given_The_Value_Error_Message()
        {
            AssertOutputContains(ExpectErrorMessage);
        }

    }

    public class When_horn_receives_a_rebuild_only_switch : CmdLineErrorSpecificationBase
    {
        protected override string[] Args
        {
            get { return new[] { "-install:castle.windsor", "-rebuildonly" }; }
        }

        protected override string ExpectErrorMessage
        {
            get { return ""; }
        }

        protected override void Because()
        {
            parser = new SwitchParser(Output, Args);

            IsValid = parser.IsAValidRequest();
        }

        [Fact]
        public void Then_the_parsed_arguments_Are_Valid()
        {
            Assert.True(IsValid);
        }

        [Fact]
        public void Then_the_install_name_should_be_castle_windsor()
        {
            Assert.Equal("castle.windsor", parser.CommandArguments.PackageName);
        }

        [Fact]
        public void Then_the_command_args_specifies_rebuild_only()
        {
            Assert.True(parser.CommandArguments.RebuildOnly);
        }
    }
}