using Horn.Core.Dsl;
using Horn.Core.Utils.Framework;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_Nant_Is_Specified_In_The_Dsl_As_The_Build_Tool : BuildWithNantSpecificationBase
    {
        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornnant.boo");
            configReader.Prepare(); 
        }

        [Fact]
        public void Then_The_Dsl_Compiles()
        {
            Assert.IsAssignableFrom<NAntBuildTool>(configReader.BuildMetaData.BuildEngine.BuildTool);

            Assert.Equal(4, configReader.BuildMetaData.BuildEngine.Tasks.Count);

            Assert.Equal(5, configReader.BuildMetaData.BuildEngine.Parameters.Count);

            Assert.True(configReader.BuildMetaData.BuildEngine.GenerateStrongKey);
        }

    }

    public class When_Nant_Is_The_Build_Tool_And_Command_Line_Arguments_Are_Requested : BuildWithNantSpecificationBase
    {
        private IBuildTool buildTool;
        private const string EXPECTED_CMD_LINE_ARGUMENTS =
            "build release quick rebuild  -t:net-3.5 -buildfile:Horn.build -D:sign=false -D:testrunner=NUnit -D:common.testrunner.enabled=true -D:common.testrunner.failonerror=true -D:build.msbuild=true";

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornnant.boo");
            configReader.Prepare();

            buildTool = configReader.BuildMetaData.BuildEngine.BuildTool;
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