using System;
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
}