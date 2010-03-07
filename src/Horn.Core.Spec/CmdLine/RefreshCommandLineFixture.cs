using Horn.Core.Utils.CmdLine;

namespace Horn.Core.Spec.Unit.CmdLine
{
    public class When_horn_receives_a_refresh_command : CmdLineSpecificationBase
    {
        private readonly string[] args = new[]{ "-refresh" };

        protected override void Because()
        {
            parser = new SwitchParser(Output, args);

            IsValid = parser.IsValid();            
        }
    }
}