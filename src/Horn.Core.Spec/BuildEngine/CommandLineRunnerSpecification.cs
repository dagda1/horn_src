using Horn.Core.BuildEngines;
using Horn.Spec.Framework.Stubs;
using Xunit;
using Rhino.Mocks;

namespace Horn.Core.Spec.BuildEngineSpecs
{
	public class When_Calling_CommandLineRunner : Specification
	{
		private IShellRunner runner;
		IProcessFactory factory;

		protected override void Because()
		{
			factory = MockRepository.GenerateMock<IProcessFactory>();
			factory.Expect(x => x.GetProcess("cmd", "foo", "bar")).Return(new StubProcess());

			runner = new CommandLineRunner(factory);
			runner.RunCommand("cmd", "foo", "bar");
		}

		[Fact]
		public void It_Should_Call_The_cmd_Process()
		{
			factory.VerifyAllExpectations();
		}
	}
}