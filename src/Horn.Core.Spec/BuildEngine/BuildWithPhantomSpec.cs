using System;
using System.IO;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils.Framework;
using Horn.Core.Utils.IoC;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.BuildEngineSpecs
{
	public class When_The_Build_MetaData_Specifies_Phantom : BuildWithPhantomSpecificationBase
	{
		private const string EXPECTED =
			"-f:Horn.boo build release quick rebuild";

		protected override void Because()
		{
			configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornphantom.boo");
			configReader.Prepare();
		}

		[Fact]
		public void The_Build_Tool_Should_be_Set_to_PhantomBuildTool()
		{
			Assert.IsType<PhantomBuildTool>(configReader.BuildMetaData.BuildEngine.BuildTool);
		}

		[Fact]
		public void It_Should_Provide_The_Correct_Args()
		{
			IBuildTool phantom = configReader.BuildMetaData.BuildEngine.BuildTool;

			var cmdLineArgs = phantom.CommandLineArguments("Horn.boo", configReader.BuildMetaData.BuildEngine, packageTree,
														FrameworkVersion.FrameworkVersion35).Trim();

			Assert.Equal(EXPECTED, cmdLineArgs);
		}

		[Fact]
		public void The_PathToBuildTool_Should_be_Phantom_exe()
		{
			IBuildTool phantom = configReader.BuildMetaData.BuildEngine.BuildTool;
			Assert.Contains("Phantom.exe", phantom.PathToBuildTool(packageTree, FrameworkVersion.FrameworkVersion35));
		}
	}

	public abstract class BuildWithPhantomSpecificationBase : Specification
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
			packageTree.Expect(x => x.Root).Return(packageTree);
			packageTree.Expect(x => x.CurrentDirectory).Return(new DirectoryInfo(Environment.CurrentDirectory));
		}
	}
}