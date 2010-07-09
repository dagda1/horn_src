using System;
using Horn.Core.Dsl;
using Horn.Core.SCM;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
	public class When_Git_Is_Specified_In_The_Dsl_For_Source_Control : GitSourceControlSpecificationBase
	{
		protected override void Because()
		{
			configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/horngit.boo");
			configReader.Prepare();
		}

		[Fact]
		public void Then_SourceControl_Should_be_Set_to_GitSourceControl()
		{
			Assert.IsAssignableFrom<GitSourceControl>(configReader.BuildMetaData.SourceControl);
		}
	}
}