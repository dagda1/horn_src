using System.IO;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils.IoC;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;

namespace Horn.Core.Spec.BuildEngineSpecs
{
    using Dsl;
    using Unit.dsl;
    using Utils.Framework;
    using Xunit;

    public class When_The_Build_MetaData_Specifies_PSake : BuildWithBatchSpecificationBase
    {
        private const string EXPECTED = "Powershell.exe";

        protected override void Because()
        {
            configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornpsake.boo");
            configReader.Prepare();
        }

        [Fact]
        public void Then_The_Batch_Build_Tool_Generates_The_Correct_Command_Line_Parameters()
        {
            IBuildTool psake = configReader.BuildMetaData.BuildEngine.BuildTool;

            var cmdLineArgs = psake.CommandLineArguments(configReader.BuildMetaData.BuildEngine.BuildFile, configReader.BuildMetaData.BuildEngine, packageTree,
                                                        FrameworkVersion.FrameworkVersion35).Trim();

            var pathToBuildFile = psake.PathToBuildTool(packageTree, FrameworkVersion.FrameworkVersion35);


            Assert.Equal(EXPECTED, pathToBuildFile);
            Assert.Equal("-command .\\default.ps1 Compile", cmdLineArgs);
        }
    }

    public abstract class BuildWithPSakeSpecificationBase : Specification
    {
        protected BooConfigReader configReader;
        protected DslFactory factory;
        protected IDependencyResolver dependencyResolver;
        protected IPackageTree packageTree;

        protected override void Before_each_spec()
        {
            dependencyResolver = CreateStub<IDependencyResolver>();
            dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(new SVNSourceControl(string.Empty));

            IoC.InitializeWith(dependencyResolver);

            factory = new DslFactory { BaseDirectory = DirectoryHelper.GetBaseDirectory() };
            factory.Register<BooConfigReader>(new ConfigReaderEngine());

            packageTree = MockRepository.GenerateStub<IPackageTree>();
        }
    }
}
