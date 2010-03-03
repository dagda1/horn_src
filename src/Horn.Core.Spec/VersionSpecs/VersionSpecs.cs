using System;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils.CmdLine;
using Horn.Core.Utils.IoC;
using Horn.Spec.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.VersionSpecs
{
    public class When_horn_receives_a_request_for_a_package_with_a_version_number : Specification
    {
        private IGet get;
        private IPackageTree packageTree;
        private PackageBuilder packageBuilder;
        private readonly string[] args = new[] { "-install:castle", "-Version:2.1.0" };
        private StringWriter textWriter;
        private MockRepository mockRepository = new MockRepository();
        private SourceControlDouble sourceControl;

        protected override void Before_each_spec()
        {
            textWriter = new StringWriter();

            var parser = new SwitchParser(textWriter, args);

            packageTree = TreeHelper.GetTempPackageTree();

            var dependencyResolver = CreateStub<IDependencyResolver>();

            sourceControl = new SourceControlDouble("http://someurl.com");

            dependencyResolver.Stub(x => x.Resolve<IBuildConfigReader>()).Return(new BooBuildConfigReader());

            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(sourceControl);

            IoC.InitializeWith(dependencyResolver);

            get = MockRepository.GenerateStub<IGet>();

            get.Stub(x => x.From(sourceControl)).Return(get);

            get.Stub(x => x.ExportTo(packageTree)).Return(packageTree);

            packageBuilder = new PackageBuilderStub(get, MockRepository.GenerateStub<IProcessFactory>(), parser.CommandArguments, parser.CommandArguments.Packages[0]);
        }

        protected override void Because()
        {
            mockRepository.Playback();

            packageBuilder.Execute(packageTree);
        }

        [Fact]
        public void Then_the_correct_build_file_is_selected()
        {
            get.AssertWasCalled(x => x.From(sourceControl));
        }

        [Fact]
        public void Then_the_correct_working_directory_is_selected()
        {
            var castle = packageTree.RetrievePackage("castle");

            castle.Version = "2.1.0";

            Assert.Equal("Working-2.1.0", castle.WorkingDirectory.Name);
        }
    }
}
