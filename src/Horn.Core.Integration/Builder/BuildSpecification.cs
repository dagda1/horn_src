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

    public class When_The_Build_Meta_Data_Specifies_A_Dependency : BuildSpecificationBase
    {
        private string dependentFilename;
        private IDependentUpdaterExecutor updaterExecutor;
        private MockRepository mockRepository;

        protected override void Because()
        {
            string rootPath = GetRootPath();

            mockRepository = new MockRepository();

            packageTree.Stub(x => x.WorkingDirectory).Return(new DirectoryInfo(workingPath));

            packageTree.Stub(x => x.Name).Return("horn");

            var solutionPath = Path.Combine(rootPath, "Horn.sln");

            updaterExecutor = CreateStub<IDependentUpdaterExecutor>();

            var dispatcher = new DependencyDispatcher(updaterExecutor);

            buildEngine = new BuildEngine(new MSBuildBuildTool(), solutionPath, FrameworkVersion.FrameworkVersion35, dispatcher);

            string dependentPath = CreateDirectory("Dependent");

            dependentFilename = "dependency.dll";

            buildEngine.BuildRootDirectory = ".";

            buildEngine.Dependencies.Add(new Dependency("dependency", "dependency"));

            var dependentTree = MockRepository.GenerateStub<IPackageTree>();

            var dependentDir = new DirectoryInfo(dependentPath);

            dependentTree.Stub(x => x.Result).Return(dependentDir);
            
            File.Create(Path.Combine(dependentPath, dependentFilename)).Close();

            packageTree.Stub(x => x.RetrievePackage("dependency")).Return(dependentTree);
        }

        [Fact]
        public void Then_The_Build_Copies_The_Dependency()
        {
            mockRepository.Playback();

            buildEngine.Build(new DiagnosticsProcessFactory(), packageTree);

            updaterExecutor.AssertWasCalled(x => x.Execute(Arg<IPackageTree>.Is.TypeOf, Arg<IEnumerable<string>>.Is.TypeOf, Arg<Dependency>.Is.TypeOf));
        }
    }
}
