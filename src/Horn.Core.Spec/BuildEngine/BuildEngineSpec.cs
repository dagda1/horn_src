using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.BuildEngineSpecs
{
    public class When_The_Build_Engine_Is_Ran : Specification
    {
        private IPackageTree packageTree;
        private IBuildTool buildToolStub;
        private BuildEngine buildEngine;

        protected override void Because()
        {
            packageTree = CreateStub<IPackageTree>();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(@"C:\"));
            
            buildToolStub = CreateStub<IBuildTool>();

            buildEngine = new BuildEngine(buildToolStub, "deeper/than/this/somebuild.file", FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            
            buildEngine.Build(new StubProcessFactory(), packageTree);
        }

        //[Fact]  Takes too long to run.  Tool long for an integration tests
        public void Then_Build_Engine_Builds_With_The_Build_Tool()
        {
            buildToolStub.AssertWasCalled(x => x.CommandLineArguments(Arg<string>.Is.Anything, Arg<BuildEngine>.Is.NotNull, Arg<IPackageTree>.Is.Anything, Arg<FrameworkVersion>.Is.Anything));
        }
    }

    public class When_The_Build_Engine_Fails : Specification
    {
        private IPackageTree packageTree;
        private IBuildTool buildToolStub;
        private BuildEngine buildEngine;

        protected override void Because()
        {
            packageTree = CreateStub<IPackageTree>();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(@"C:\"));

            buildToolStub = CreateStub<IBuildTool>();

            buildEngine = new BuildEngine(buildToolStub, "deeper/than/this/somebuild.file", FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            buildEngine.Build(new StubProcessFactory(), packageTree);
        }
    }

    public class When_The_Build_Engine_Receives_An_Array_Of_Parameters : Specification
    {
        private readonly string[] switches = new[] { "sign=false", "testrunner=NUnit", "common.testrunner.enabled=true", "common.testrunner.failonerror=true", "build.msbuild=true" };
        private BuildEngine buildEngine;

        protected override void Because()
        {            
            buildEngine = new BuildEngine(null, "", FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            buildEngine.AssignParameters(switches);
        }

        [Fact]
        public void Then_A_Dictionary_Of_Switches_Is_Created()
        {
            Assert.Equal(5, buildEngine.Parameters.Keys.Count);
        }
    }
}
