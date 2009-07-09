namespace Horn.Core.Spec.Dependencies
{
    using System.Collections.Generic;
    using BuildEngines;
    using Core.Dependencies;
    using PackageStructure;
    using Rhino.Mocks;
    using Xunit;

    public class when_executing_the_dependent_updaters_and_there_are_dependencies_to_update : dependent_updater_executor_context
    {
        protected override void Before_each_spec()
        {
            packageTree = CreateStub<IPackageTree>();
            dependency = new Dependency("Test", "Test");
            updater = CreateStub<IDependentUpdater>();
            dependencyPaths = new[] {"path"};
            executor = new DependentUpdaterExecutor(new[] { updater });
        }

        protected override void Because()
        {
            executor.Execute(packageTree, dependencyPaths, dependency);
        }


        [Fact]
        public void should_execute_the_updater()
        {
            updater.AssertWasCalled(u => u.Update(Arg<DependentUpdaterContext>.Is.Anything));
        }
    }

    public class when_executing_the_dependent_updaters_and_there_are_no_dependencies_to_update : dependent_updater_executor_context
    {
        protected override void Before_each_spec()
        {
            packageTree = CreateStub<IPackageTree>();
            dependency = new Dependency("Test", "Test");
            updater = CreateStub<IDependentUpdater>();
            dependencyPaths = new List<string>();
            executor = new DependentUpdaterExecutor(new[] { updater });
        }

        protected override void Because()
        {
            executor.Execute(packageTree, dependencyPaths, dependency);
        }

        [Fact]
        public void should_not_execute_the_updater()
        {
            updater.AssertWasNotCalled(u => u.Update(Arg<DependentUpdaterContext>.Is.Anything));
        }
    }
}