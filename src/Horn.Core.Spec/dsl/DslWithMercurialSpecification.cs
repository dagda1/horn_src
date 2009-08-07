using System;
using Horn.Core.BuildEngines;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Unit.dsl
{
	public class When_HG_Is_Specified_In_The_Dsl_For_Source_Control : MercurialSourceControlSpecificationBase
	{
		protected override void Because()
		{
			configReader = factory.Create<BooConfigReader>(@"BuildConfigs/Horn/hornhg.boo");
			configReader.Prepare();
		}

		[Fact]
		public void Then_SourceControl_Should_be_Set_to_MercurialSourceControl()
		{
			Assert.IsAssignableFrom<MercurialSourceControl>(configReader.BuildMetaData.SourceControl);
		}
	}

	public abstract class MercurialSourceControlSpecificationBase : Specification
	{
		protected BooConfigReader configReader;
		protected DslFactory factory;
		protected IDependencyResolver dependencyResolver;
		protected IPackageTree packageTree;

		protected override void Before_each_spec()
		{
			dependencyResolver = CreateStub<IDependencyResolver>();
			var environmentVariable = CreateStub<IEnvironmentVariable>();
			environmentVariable.Stub(x => x.GetDirectoryFor("hg.exe")).Return(Environment.CurrentDirectory);
			dependencyResolver.Stub(x => x.Resolve<MercurialSourceControl>()).Return(new MercurialSourceControl(CreateStub<IShellRunner>(), environmentVariable));

			IoC.InitializeWith(dependencyResolver);

			factory = new DslFactory { BaseDirectory = DirectoryHelper.GetBaseDirectory() };
			factory.Register<BooConfigReader>(new ConfigReaderEngine());

			packageTree = MockRepository.GenerateStub<IPackageTree>();
		}
	}
}