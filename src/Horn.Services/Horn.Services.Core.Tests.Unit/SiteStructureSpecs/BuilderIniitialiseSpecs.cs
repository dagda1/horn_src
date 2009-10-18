using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Services.Core.Builder;
using Horn.Services.Core.Tests.Unit.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.PackageTreeBuilderSpecs
{
    public class When_the_package_tree_builder_is_initialised : BuilderSpecBase
    {
        private ISiteStructureBuilder siteStructureBuilder;
        private IMetaDataSynchroniser metaDataSynchroniser;
        private IFileSystemProvider fileSystemProvider;

        public override void before_each_spec()
        {
            base.before_each_spec();

            hornDirectory = FileSystemHelper.GetFakeDummyHornDirectory();

            metaDataSynchroniser = MockRepository.GenerateStub<IMetaDataSynchroniser>();

            fileSystemProvider = MockRepository.GenerateStub<IFileSystemProvider>();

            fileSystemProvider.Stub(x => x.GetHornRootDirectory(Arg<string>.Is.TypeOf)).Return(FileSystemHelper.GetFakeDummyHornDirectory());

            dependencyResolver.Stub(x => x.Resolve<IPackageTree>());

            siteStructureBuilder = new SiteStructureBuilder(metaDataSynchroniser, fileSystemProvider, new DirectoryInfo(@"C:\").FullName);

            siteStructureBuilder.Initialise();
        }

        protected override void establish_context()
        {
        }

        protected override void because()
        {
        }

        [Test]
        public void Then_the_package_tree_is_downloaded()
        {
            metaDataSynchroniser.AssertWasCalled(x => x.SynchronisePackageTree(Arg<IPackageTree>.Is.TypeOf));
        }

        [Test]
        public void Then_the_sandbox_directory_is_created()
        {
            fileSystemProvider.AssertWasCalled(x => x.CreateTemporaryHornDirectory(Arg<string>.Is.TypeOf));
        }
    }
}