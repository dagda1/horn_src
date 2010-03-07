using Horn.Core.Dsl;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_Horn_Parses_A_Successful_Configuration : BaseDSLSpecification
    {
        private IBuildMetaData buildMetaData;

        protected override void Because()
        {
            buildMetaData = GetBuildMetaDataInstance();
        }

        [Fact]
        public void Then_A_Build_Meta_Data_Object_Is_Created()
        {
            AssertBuildMetaDataValues(buildMetaData);

        }
    }

    public class ConfigReaderDouble : BooConfigReader
    {
        public override void Prepare()
        {
        }
    }
}