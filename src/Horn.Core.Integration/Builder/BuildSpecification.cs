using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Utils.Framework;
using Rhino.Mocks;
using Xunit;
using Horn.Core.PackageStructure;

namespace Horn.Core.Integration.Builder
{
    using Dependencies;

    public class When_The_Build_Meta_Data_Specifies_MSBuild : BuildSpecificationBase
    {
        protected override void Because()
        {
            string rootPath = GetRootPath();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(workingPath));

            packageTree.Stub(x => x.Name).Return("horn");

            var solutionPath = Path.Combine(Path.Combine(rootPath, "Horn.Core"), "Horn.Core.csproj");

            buildEngine = new BuildEngine(new MSBuildBuildTool(), solutionPath, FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>()){BuildRootDirectory = "."};
        }

        [Fact]
        public void Then_MSBuild_Compiles_The_Source()
        {
            buildEngine.Build(new DiagnosticsProcessFactory(), packageTree);

            Assert.True(File.Exists(Path.Combine(outputPath, "Horn.Core.dll")));
        }
    }
}
