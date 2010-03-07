using System.IO;
using Horn.Core.Utils.CmdLine;
using Xunit;


namespace Horn.Core.Spec.Unit.CmdLine
{
    public abstract class CmdLineSpecificationBase : Specification
    {
        private TextWriter textWriter;
        protected SwitchParser parser;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();

            textWriter = new StringWriter();
        }

        protected void AssertOutputContains(string outoutShouldContain)
        {
            Assert.True(Output.ToString().Contains(outoutShouldContain));
        }

        protected TextWriter Output { get { return textWriter; } }
        protected bool IsValid { get; set; }

        protected void AssertIsValid()
        {
            Assert.True(IsValid, Output.ToString());
        }
    }


    public abstract class CmdLineErrorSpecificationBase : CmdLineSpecificationBase
    {
        protected override void Because()
        {
            parser = new SwitchParser(Output, Args);
            IsValid = parser.IsValid();
        }

        protected abstract string[] Args { get; }
        protected abstract string ExpectErrorMessage { get; }
    }
}