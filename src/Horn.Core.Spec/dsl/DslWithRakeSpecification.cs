using Horn.Core.Dsl;
using Horn.Core.Utils;
using Horn.Core.Utils.Framework;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_Rake_Is_Specified_In_The_Dsl_As_The_Build_Tool : BuildWithNantSpecificationBase
    {
        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornrake.boo");
            configReader.Prepare();
        }

        [Fact]
        public void Then_The_Dsl_Compiles()
        {
            Assert.IsAssignableFrom<RakeBuildTool>(configReader.BuildMetaData.BuildEngine.BuildTool);

            Assert.Equal(4, configReader.BuildMetaData.BuildEngine.Tasks.Count);

            Assert.Equal(5, configReader.BuildMetaData.BuildEngine.Parameters.Count);

            Assert.True(configReader.BuildMetaData.BuildEngine.GenerateStrongKey);
        }

    }

    public class When_Rake_Is_The_Build_Tool_And_Command_Line_Arguments_Are_Requested : BuildWithNantSpecificationBase
    {
        private IBuildTool buildTool;
        private const string EXPECTED_CMD_LINE_ARGUMENTS =
            @"C:\Ruby\bin\rake --rakefile Horn.build sign=false testrunner=NUnit common.testrunner.enabled=true common.testrunner.failonerror=true build.msbuild=true build release quick rebuild";

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornrake.boo");
            configReader.Prepare();

            //buildTool = configReader.BuildMetaData.BuildEngine.BuildTool;
            var variable = MockRepository.GenerateStub<IEnvironmentVariable>();
            variable.Expect(x => x.GetDirectoryFor("ruby.exe")).Return(@"C:\Ruby\bin");
            buildTool = new RakeBuildTool(variable);
        }

        [Fact]
        public void Then_The_Build_Tool_Renders_The_Expected_Arguments()
        {
            var actual = buildTool.CommandLineArguments("Horn.build", configReader.BuildMetaData.BuildEngine, packageTree,
                                                        FrameworkVersion.FrameworkVersion35);

            Assert.Equal(EXPECTED_CMD_LINE_ARGUMENTS, actual);
        }
    }
}