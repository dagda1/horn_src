namespace Horn.Core.Spec.BuildEngineSpecs
{
    using Dsl;
    using Unit.dsl;
    using Utils.Framework;
    using Xunit;

    public class When_The_Build_MetaData_Specifies_Batch : BuildWithBatchSpecificationBase
    {
        private const string EXPECTED =
            "src/build.bat";

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornbatch.boo");
            configReader.Prepare();
        }

        [Fact]
        public void Then_The_Batch_Build_Tool_Generates_The_Correct_Command_Line_Parameters()
        {
            IBuildTool batch = configReader.BuildMetaData.BuildEngine.BuildTool;
            
            var cmdLineArgs = batch.CommandLineArguments(configReader.BuildMetaData.BuildEngine.BuildFile, configReader.BuildMetaData.BuildEngine, packageTree,
                                                        FrameworkVersion.FrameworkVersion35).Trim();

            Assert.Equal(EXPECTED, cmdLineArgs);
        }
    }
}
