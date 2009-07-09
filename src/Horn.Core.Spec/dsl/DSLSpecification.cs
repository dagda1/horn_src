using System;
using Horn.Core.Dsl;
using Horn.Core.SCM;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
    public class When_Horn_Receives_A_Request_For_A_Component : BaseDSLSpecification
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
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/horn.boo");
            configReader.Prepare();
        }

        private void AssertHornMetaData(BooConfigReader reader)
        {
            Assert.NotNull(reader);

            Assert.Equal("horn", reader.BuildMetaData.InstallName);

            Assert.Equal(Description, reader.BuildMetaData.Description);

            Assert.IsAssignableFrom<SVNSourceControl>(reader.BuildMetaData.SourceControl);

            Assert.Equal(SvnUrl, reader.BuildMetaData.SourceControl.Url);

            Assert.IsAssignableFrom<MSBuildBuildTool>(reader.BuildMetaData.BuildEngine.BuildTool);

            Assert.Equal(BuildFile, reader.BuildMetaData.BuildEngine.BuildFile);

            Assert.Equal(".", reader.BuildMetaData.BuildEngine.SharedLibrary);

            Assert.Equal("Output", reader.BuildMetaData.BuildEngine.BuildRootDirectory);

            Assert.Equal(5, reader.BuildMetaData.BuildEngine.Dependencies.Count);

            Assert.Equal("log4net", reader.BuildMetaData.BuildEngine.Dependencies[0].PackageName);

            Assert.Equal("lib", reader.BuildMetaData.BuildEngine.Dependencies[0].Library);

            Assert.Equal(3, reader.PackageMetaData.PackageInfo.Count);
        }

        [Fact]
        public void Then_Horn_Returns_The_Component_DSL()
        {
            AssertHornMetaData(configReader);
        }

        [Fact]
        public void Should_Resolve_The_Appropriate_SourceControl()
        {
            dependencyResolver.AssertWasCalled(r => r.Resolve<SVNSourceControl>());
        }
    }
}