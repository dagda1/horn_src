using System;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Spec.Unit.dsl;
using Horn.Core.Utils.Framework;
using Horn.Core.Utils.IoC;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.BuildEngineSpecs
{
	public class When_The_Build_MetaData_Specifies_PSake : BuildWithBatchSpecificationBase
	{
		//private const string EXPECTED = "Powershell.exe";
		private string teamcityVersion = "";

		protected override void Because()
		{
			// reset TEAMCITY_VERSION in case we are running the test on a TC environment.
			teamcityVersion = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");						
			Environment.SetEnvironmentVariable("TEAMCITY_VERSION", "");
			configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornpsake.boo");
			configReader.Prepare();
		}

		protected override void After_each_spec()
		{
			// restore the TC environment.
			Environment.SetEnvironmentVariable("TEAMCITY_VERSION", teamcityVersion);
		}

		[Fact]
		public void Then_The_Batch_Build_Tool_Generates_The_Correct_Command_Line_Parameters()
		{
			IBuildTool psake = configReader.BuildMetaData.BuildEngine.BuildTool;

			string cmdLineArgs =
				psake.CommandLineArguments(configReader.BuildMetaData.BuildEngine.BuildFile, configReader.BuildMetaData.BuildEngine,
				                           packageTree,
				                           FrameworkVersion.FrameworkVersion35).Trim();

			string pathToBuildFile = psake.PathToBuildTool(packageTree, FrameworkVersion.FrameworkVersion35);


			//Assert.Equal(EXPECTED, pathToBuildFile);
			Assert.Equal("-command .\\psake default.ps1 Compile", cmdLineArgs);
		}
	}

	public class When_The_Build_MetaData_Specifies_PSake_and_the_build_environment_is_TeamCity : BuildWithBatchSpecificationBase
	{
		protected override void Because()
		{
			Environment.SetEnvironmentVariable("TEAMCITY_VERSION", "31337");
			configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornpsake.boo");
			configReader.Prepare();			
		}

		[Fact]
		public void Then_the_batch_build_tool_generates_psake_command()
		{
			IBuildTool psake = configReader.BuildMetaData.BuildEngine.BuildTool;
			string cmdLineArgs =
				psake.CommandLineArguments(configReader.BuildMetaData.BuildEngine.BuildFile, configReader.BuildMetaData.BuildEngine,
										   packageTree,
										   FrameworkVersion.FrameworkVersion35).Trim();			

			Assert.Contains("-command .\\psake default.ps1 Compile", cmdLineArgs);
		}

		[Fact]
		public void Then_the_batch_build_tool_generates_a_cmd_hosted_command()
		{
			IBuildTool psake = configReader.BuildMetaData.BuildEngine.BuildTool;
			string cmdLineArgs =
				psake.CommandLineArguments(configReader.BuildMetaData.BuildEngine.BuildFile, configReader.BuildMetaData.BuildEngine,
										   packageTree,
										   FrameworkVersion.FrameworkVersion35).Trim();
			string pathToBuildFile = psake.PathToBuildTool(packageTree, FrameworkVersion.FrameworkVersion35);

			Assert.Equal("cmd.exe", pathToBuildFile);
		}
	}

	public abstract class BuildWithPSakeSpecificationBase : Specification
	{
		protected BooConfigReader configReader;
		protected IDependencyResolver dependencyResolver;
		protected DslFactory factory;
		protected IPackageTree packageTree;

		protected override void Before_each_spec()
		{
			dependencyResolver = CreateStub<IDependencyResolver>();
			dependencyResolver.Stub(x => x.Resolve<SVNSourceControl>()).Return(new SVNSourceControl(string.Empty));

			IoC.InitializeWith(dependencyResolver);

			factory = new DslFactory {BaseDirectory = DirectoryHelper.GetBaseDirectory()};
			factory.Register<BooConfigReader>(new ConfigReaderEngine());

			packageTree = MockRepository.GenerateStub<IPackageTree>();
		}
	}
}