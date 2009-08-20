using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.Framework;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.BuildEngineSpecs
{
	public class When_Calling_MSBuildBuildTool : Specification
	{
		private MSBuildBuildTool runner;
		private BuildEngine buildEngine;
		private IPackageTree packageTree;

		protected override void Because()
		{
			buildEngine = new BuildEngine(null, "", FrameworkVersion.FrameworkVersion35, 
				CreateStub<IDependencyDispatcher>());
			buildEngine.BuildRootDirectory = "output";
			packageTree = MockRepository.GenerateStub<IPackageTree>();
			packageTree.Expect(pt => pt.WorkingDirectory).Return(new DirectoryInfo("C:\\temp"));

			runner = new MSBuildBuildTool();
		}

		[Fact]
		public void It_Should_Build_CommandLine()
		{
			var result = runner.CommandLineArguments("abc.sln",
													 buildEngine,
													 packageTree, FrameworkVersion.FrameworkVersion35);

			Assert.Equal(
				"\"abc.sln\" /p:OutputPath=\"C:\\temp\\output\"  /p:TargetFrameworkVersion=v3.5 /p:NoWarn=1591 /consoleloggerparameters:Summary",
				result);
		}

		[Fact]
		public void It_Should_Process_Parameters()
		{
			buildEngine.AssignParameters(new[] { "/p:SOMEVAR=true" });

			var result = runner.CommandLineArguments("abc.sln",
													 buildEngine,
													 packageTree, FrameworkVersion.FrameworkVersion35);

			Assert.Equal(
				"\"abc.sln\" /p:OutputPath=\"C:\\temp\\output\"  /p:TargetFrameworkVersion=v3.5 /p:NoWarn=1591 /consoleloggerparameters:Summary /p:SOMEVAR=true",
				result);
		}

		[Fact]
		public void It_Should_Process_Tasks()
		{
			buildEngine.AssignTasks(new[] {"clean"});

			var result = runner.CommandLineArguments("abc.sln",
													 buildEngine,
													 packageTree, FrameworkVersion.FrameworkVersion35);

			Assert.Equal(
				"\"abc.sln\" /p:OutputPath=\"C:\\temp\\output\"  /p:TargetFrameworkVersion=v3.5 /p:NoWarn=1591 /consoleloggerparameters:Summary /t:clean",
				result);
		}
	}
}
