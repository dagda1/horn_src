using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Spec.BuildEngineSpecs;
using Horn.Core.Spec.Unit.GetSpecs;
using Horn.Core.Utils;
using Horn.Core.Utils.Framework;
using Horn.Framework.helpers;
using Horn.Spec.Framework.doubles;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;
using BuildToolStub=Horn.Core.Spec.BuildEngineSpecs.BuildToolStub;

namespace Horn.Core.Spec.Unit.PackageCommands
{
    public class When_The_Builder_Receives_An_Install_Switch : Specification
    {
        protected IGet get;
        protected IBuildConfigReader buildConfigReader;
        protected IPackageTree wholeTree;
        protected IFileSystemProvider fileSystemProvider;

        protected override void Because()
        {
            get = new Get(fileSystemProvider);

            IBuildMetaData buildMetaData;

            IPackageTree componentTree = GetComponentTree(out buildMetaData);

            wholeTree = CreateStub<IPackageTree>();

            wholeTree.Stub(x => x.Name).Return("horn");

            wholeTree.Stub(x => x.BuildNodes()).Return(new List<IPackageTree> {wholeTree});

            wholeTree.Stub(x => x.RetrievePackage(CommandLineHelper.GetCommandLineArgs("horn"))).Return(componentTree).IgnoreArguments();

            wholeTree.Stub(x => x.GetBuildMetaData("horn")).Return(buildMetaData).IgnoreArguments().Repeat.Any();

            wholeTree.Stub(x => x.Result).Return(new DirectoryInfo(@"C:\somewhere\build_root_dir"));
        }

        private IPackageTree GetComponentTree(out IBuildMetaData buildMetaData)
        {
            var baseConfigReader = CreateStub<BooConfigReader>();

            baseConfigReader.BuildMetaData.InstallName = "horn";

            var componentTree = CreateStub<IPackageTree>();

            var tempDirectory = new DirectoryInfo(DirectoryHelper.GetTempDirectoryName());

            componentTree.Stub(x => x.WorkingDirectory).Return(tempDirectory).Repeat.Any();

            componentTree.Stub(x => x.GetRevisionData()).Return(new RevisionData("3"));

            buildMetaData = GetBuildMetaData(baseConfigReader);

            componentTree.Stub(x => x.GetBuildMetaData("horn")).Return(buildMetaData);

            componentTree.Stub(x => x.Name).Return("log4net");

            componentTree.Stub(x => x.Result).Return(new DirectoryInfo(@"C:\somewhere\build_root_dir"));

            componentTree.Stub(x => x.GetBuildMetaData("log4net"))
                         .Return(buildMetaData).IgnoreArguments().Repeat.Any();

            return componentTree;
        }

        private IBuildMetaData GetBuildMetaData(BooConfigReader baseConfigReader)
        {
            var buildTool = new BuildToolStub();

            var buildEngine = new BuildEngineStub(buildTool, "Test", FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            baseConfigReader.BuildMetaData.BuildEngine = buildEngine;

            var buildMetaData = CreateStub<IBuildMetaData>();

            buildMetaData.SourceControl = new SourceControlDoubleWithFakeFileSystem("Svn://some.url");

            buildMetaData.BuildEngine = buildEngine;

            return buildMetaData;
        }

        [Fact]
        public void Then_The_Builder_Coordinates_The_Build()
        {
            IPackageCommand command = new PackageBuilder(get, new StubProcessFactory(), new CommandArgsDouble("horn"));

            command.Execute(wholeTree);
        }
    }

    public class When_the_package_builder_receives_an_install_command_for_an_unknown_package : GetSpecificationBase
    {
        private PackageBuilder packageBuilder;

        protected override void Because()
        {
            packageBuilder = new PackageBuilder(get, MockRepository.GenerateStub<IProcessFactory>(), new CommandArgsDouble("unknownpackage"));

            packageTree = TreeHelper.GetTempPackageTree();
        }

        [Fact]
        public void Then_an_unknown_package_exception_is_thrown()
        {
            Assert.Throws<UnknownInstallPackageException>(() => packageBuilder.Execute(packageTree));
        }
    }

    public class When_the_package_builder_receives_a_rebuild_only_switch : GetSpecificationBase
    {
        private PackageBuilder packageBuilder;
        private MockRepository mockRepository;

        protected override void Before_each_spec()
        {
            mockRepository = new MockRepository();

            packageTree = new PackageTreeStub(TreeHelper.GetPackageTreeParts(new List<Dependency>()), "log4net", false);

            get = MockRepository.GenerateStub<IGet>();

            get.Stub(x => x.From(new SVNSourceControl("url"))).Return(get);

            get.Stub(x => x.ExportTo(packageTree)).Return(packageTree);

            packageBuilder = new PackageBuilder(get, MockRepository.GenerateStub<IProcessFactory>(), new CommandArgsDouble("log4net", true));
        }

        protected override void Because()
        {
            mockRepository.Playback();
            
            packageBuilder.Execute(packageTree);
        }

        [Fact]
        public void Then_source_control_get_is_not_called()
        {
            get.AssertWasNotCalled(x => x.From(Arg<SVNSourceControl>.Is.TypeOf));
        }
    }
}