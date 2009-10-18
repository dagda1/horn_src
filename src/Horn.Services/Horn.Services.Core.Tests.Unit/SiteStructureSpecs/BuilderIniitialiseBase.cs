using System.IO;
using Horn.Core.Dsl;
using Horn.Core.PackageCommands;
using Horn.Core.SCM;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Core.Utils.IoC;
using Horn.Services.Core.Builder;
using Horn.Services.Core.Config;
using Horn.Services.Core.Tests.Unit.Helpers;
using horn.services.core.Value;
using Horn.Spec.Framework;
using Horn.Spec.Framework.Stubs;
using NUnit.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.SiteStructureSpecs
{
    public abstract class BuilderSpecBase : ContextSpecification
    {
        protected IMetaDataSynchroniser metaDataSynchroniser;
        protected SiteStructureBuilder siteStructureBuilder;
        protected IFileSystemProvider fileSystemProvider;
        protected IPackageCommand packageBuilder;

        public override void before_each_spec()
        {
            var dependencyResolver = MockRepository.GenerateStub<IDependencyResolver>();
            metaDataSynchroniser = MockRepository.GenerateStub<IMetaDataSynchroniser>();
            fileSystemProvider = MockRepository.GenerateStub<IFileSystemProvider>();
            packageBuilder = MockRepository.GenerateStub<IPackageCommand>();

            var configReader = new BooBuildConfigReader();

            dependencyResolver.Stub(x => x.Resolve<IBuildConfigReader>()).Return(configReader);

            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(
                new SourceControlDouble("http://someurl.com/"));

            dependencyResolver.Stub(x => x.Resolve<IPackageCommand>("install")).Return(packageBuilder);

            fileSystemProvider.Stub(x => x.GetTemporaryBuildDirectory(Arg<DirectoryInfo>.Is.TypeOf)).Return(
                new DirectoryInfo(@"C:\temp\build"));

            IoC.InitializeWith(dependencyResolver);

            fileSystemProvider.Stub(x => x.GetHornRootDirectory(Arg<string>.Is.TypeOf)).Return(FileSystemHelper.GetFakeDummyHornDirectory());

            fileSystemProvider.Stub(x => x.CreateTemporaryHornDirectory(Arg<string>.Is.TypeOf)).Return(new DirectoryInfo(HornConfig.Settings.HornTempDirectory));

            fileSystemProvider.Stub(x => x.ZipFolder(Arg<DirectoryInfo>.Is.TypeOf, Arg<DirectoryInfo>.Is.TypeOf, Arg<string>.Is.TypeOf)).Return(
                new FileInfo(@"C:\zip"));

            siteStructureBuilder = GetSiteBuilder();

            siteStructureBuilder.Initialise();

            siteStructureBuilder.Build();
        }

        protected abstract SiteStructureBuilder GetSiteBuilder();

        protected void AssertCategoryIntegrity(Category category)
        {
            Assert.That(category.Name, Is.EqualTo("loggers"));

            Assert.That(category.Categories.Count, Is.EqualTo(1));

            var log4net = category.Categories[0];

            Assert.That(log4net.Packages.Count, Is.EqualTo(2));

            Assert.That(log4net.Packages[0].Name, Is.EqualTo("log4net"));

            Assert.That(log4net.Packages[0].Version, Is.EqualTo("1.2.10"));
        }

        protected override void because()
        {
        }

        protected override void establish_context()
        {
        }
    }
}