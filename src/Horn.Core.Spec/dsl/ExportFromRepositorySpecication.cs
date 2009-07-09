using Horn.Core.Dsl;
using Horn.Core.SCM;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_retrieving_from_a_repository : Specification
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
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/repository.boo");
            configReader.Prepare();
        }


        [Fact]
        public void Then_the_model_contains_the_repository_details()
        {
            Assert.Equal("castle", configReader.BuildMetaData.RepositoryElementList[0].RepositoryName);
            Assert.Equal("here", configReader.BuildMetaData.RepositoryElementList[0].IncludePath);
            Assert.Equal("there", configReader.BuildMetaData.RepositoryElementList[0].ExportPath);
            Assert.Equal("castle", configReader.BuildMetaData.RepositoryElementList[1].RepositoryName);
            Assert.Equal("over", configReader.BuildMetaData.RepositoryElementList[1].IncludePath);
            Assert.Equal("out", configReader.BuildMetaData.RepositoryElementList[1].ExportPath);
        }

    }

    public class When_we_need_an_include_list : Specification
    {
        private const string RepositoryName = "repository";
        private const string IncludePath = "here";
        private const string ExportPath = "there";
        private RepositoryElement _repositoryElement;

        protected override void Because()
        {
            _repositoryElement = new RepositoryElement(RepositoryName, IncludePath, ExportPath);
        }

        [Fact]
        public void Then_the_model_can_express_this()
        {
            Assert.Equal(RepositoryName, _repositoryElement.RepositoryName);
            Assert.Equal(IncludePath, _repositoryElement.IncludePath);
            Assert.Equal(ExportPath, _repositoryElement.ExportPath);
        }
    }
}