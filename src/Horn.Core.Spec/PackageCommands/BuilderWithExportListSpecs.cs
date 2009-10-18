using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.SCM;
using Horn.Core.Spec.Unit.GetSpecs;
using Horn.Core.Utils;
using Horn.Spec.Framework.doubles;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.PackageCommands
{
    public class When_the_meta_data_has_an_export_list : GetSpecificationBase
    {
        private PackageBuilder packageBuilder;
        private MockRepository mockRepository;
        private readonly SourceControlDouble sourceControlDouble = new SourceControlDouble("url1");

        protected override void Before_each_spec()
        {
            mockRepository = new MockRepository();

            var exportList = new List<SourceControl> { sourceControlDouble };

            packageTree = new PackageTreeStub(TreeHelper.GetPackageTreeParts(new List<Dependency>(), exportList), "log4net", false);

            get = new Get(MockRepository.GenerateStub<IFileSystemProvider>());

            packageBuilder = new PackageBuilder(get, new StubProcessFactory(), new CommandArgsDouble("log4net"));
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
        public void Then_the_export_list_is_retrieved()
        {
            Assert.True(sourceControlDouble.FileWasDownloaded);
        }
    }
}