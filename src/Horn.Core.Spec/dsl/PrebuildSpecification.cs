using Horn.Core.Dsl;
using Horn.Core.SCM;
using Horn.Core.Utils.IoC;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_the_build_file_contains_a_prebuild_step : BaseDSLSpecification
    {
        private BooConfigReader configReader;
        protected DslFactory factory;
        private IDependencyResolver dependencyResolver;

        protected override void Before_each_spec()
        {
            dependencyResolver = CreateStub<IDependencyResolver>();
            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>())
                .Return(new SVNSourceControl(string.Empty));

            IoC.InitializeWith(dependencyResolver);

            var engine = new ConfigReaderEngine();

            factory = new DslFactory { BaseDirectory = DirectoryHelper.GetBaseDirectory() };
            factory.Register<BooConfigReader>(engine);
        }

        protected override void After_each_spec()
        {
            IoC.InitializeWith(null);
        }

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornprebuild.boo");
            configReader.Prepare();
        }

        [Fact]
        public void Then_the_cmd_is_executed()
        {
            Assert.Equal("dir", configReader.BuildMetaData.PrebuildCommandList[0]);
            Assert.Equal("@echo \"hello\"", configReader.BuildMetaData.PrebuildCommandList[1]);
            Assert.Equal(2, configReader.BuildMetaData.PrebuildCommandList.Count);
        }
    }
}