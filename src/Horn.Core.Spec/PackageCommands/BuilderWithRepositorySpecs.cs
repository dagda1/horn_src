using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.Spec.Unit.GetSpecs;
using Horn.Core.Utils;
using Horn.Spec.Framework.doubles;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.PackageCommands
{
    public class When_the_metadata_has_a_repository_get : GetSpecificationBase
    {
        private PackageBuilder packageBuilder;
        private MockRepository mockRepository;
        private readonly SourceControlDouble sourceControlDouble = new SourceControlDouble("url1");
        private IRepositoryElement castleElementOne;

        protected override void Before_each_spec()
        {
            mockRepository = new MockRepository();

            castleElementOne = MockRepository.GenerateStub<IRepositoryElement>();

            castleElementOne.Stub(x => x.PrepareRepository(null, null)).Return(castleElementOne).IgnoreArguments();

            var repositoryIncludes = new List<IRepositoryElement> {castleElementOne};

            packageTree = new PackageTreeStub(TreeHelper.GetPackageTreeParts(new List<Dependency>(), repositoryIncludes), "castle", false);

            get = new Get(MockRepository.GenerateStub<IFileSystemProvider>());

            packageBuilder = new PackageBuilder(get, new StubProcessFactory(), new CommandArgsDouble("castle"));
        }

        protected override void Because()
        {
            mockRepository.Playback();

            packageBuilder.Execute(packageTree);
        }

        protected override void After_each_spec()
        {
            sourceControlDouble.Dispose();
        }

        [Fact]
        public void Then_the_parts_are_retrieved_from_the_repository()
        {
            castleElementOne.AssertWasCalled(x => x.Export());
        }
    }
}