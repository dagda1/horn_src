using System;
using System.IO;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils;
using Horn.Framework.helpers;
using Rhino.DSL;
using Rhino.Mocks;

namespace Horn.Core.Spec.Unit.dsl
{
	public abstract class GitSourceControlSpecificationBase : Specification
	{
		protected BooConfigReader configReader;
		protected DslFactory factory;
		protected IDependencyResolver dependencyResolver;
		protected IPackageTree packageTree;

		protected override void Before_each_spec()
		{
			dependencyResolver = CreateStub<IDependencyResolver>();
			var environmentVariable = CreateStub<IEnvironmentVariable>();
			environmentVariable.Stub(x => x.GetDirectoryFor("git.exe")).Return(Environment.CurrentDirectory);
			dependencyResolver.Stub(x => x.Resolve<GitSourceControl>()).Return(new GitSourceControl(environmentVariable));

			IoC.InitializeWith(dependencyResolver);

			factory = new DslFactory { BaseDirectory = DirectoryHelper.GetBaseDirectory() };
			factory.Register<BooConfigReader>(new ConfigReaderEngine());

			packageTree = MockRepository.GenerateStub<IPackageTree>();
		}
	}
}