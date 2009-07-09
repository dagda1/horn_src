using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Spec.BuildEngineSpecs;
using Horn.Core.Spec.Doubles;
using Horn.Core.Spec.helpers;
using Horn.Core.Utils.CmdLine;
using Horn.Spec.Framework.doubles;
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

            packageBuilder = new PackageBuilderStub(get, MockRepository.GenerateStub<IProcessFactory>(), parser.CommandArguments);
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
    }
}
