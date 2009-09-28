namespace Horn.Core.Spec.Unit.dsl
{
    using System;
    using Core.SCM;
    using Dsl;
    using Framework.helpers;
    using PackageStructure;
    using Rhino.DSL;
    using Rhino.Mocks;

    public abstract class BuildWithBatchSpecificationBase : Specification
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
