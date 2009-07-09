using System;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Utils.Framework;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Integration.Builder
{
    using Dependencies;

    public class When_The_Build_MetaData_Specifies_Nant : BuildSpecificationBase
    {
        protected string rootPath;

        protected override void Because()
        {
            rootPath = GetRootPath();

            var path = Path.Combine(rootPath, "horn.build");

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(workingPath)).Repeat.Once();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(rootPath)).Repeat.Once();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(workingPath)).Repeat.Once();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(workingPath)).Repeat.Once();

            buildEngine = new BuildEngine(new NAntBuildTool(), path, FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            var nant = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Nant.exe"));

            packageTree.Stub(x => x.Nant).Return(nant);

            buildEngine.AssignParameters(new[] { "sign=false", "testrunner=NUnit", "common.testrunner.enabled=true", "common.testrunner.failonerror=true", "build.msbuild=true"});

            buildEngine.AssignTasks(new[]{ "build "});
        }

        //TODO: Simply make this pass
        //[Fact]
        public void Then_Nant_Builds_The_Source()
        {
            buildEngine.Build(new DiagnosticsProcessFactory(), packageTree);

            //TEST DOES NOT PASS
            Assert.True(File.Exists(Path.Combine(outputPath, "Horn.Core.dll")));
        }
    }
}