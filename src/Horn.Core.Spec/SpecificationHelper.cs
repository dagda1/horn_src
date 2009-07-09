using Horn.Core.Dsl;
using Horn.Core.Spec.Unit.dsl;

namespace Horn.Core.Spec
{
    public static class SpecificationHelper
    {
        public static IBuildMetaData GetBuildMetaData()
        {
            var buildMetaData = BaseDSLSpecification.GetBuildMetaDataInstance();

            return buildMetaData;
        }
    }
}