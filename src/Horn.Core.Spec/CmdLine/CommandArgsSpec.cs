using System;
using Horn.Core.Utils.CmdLine;
using Xunit;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_the_command_line_arguments_are_parsed : CmdLineSpecificationBase
    {
        private readonly string[] args = new[] { "-install:horn" };
        private const string installName = "horn";

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);
        }

        [Fact]
        public void Then_a_command_args_object_is_created()
        {
            Assert.Equal(installName, parser.CommandArguments.PackageName);

            Assert.False(parser.CommandArguments.RebuildOnly);
        }
    }
}