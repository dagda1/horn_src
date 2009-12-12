using System;
using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.Spec.BuildEngineSpecs;
using Rhino.Mocks;
using Xunit;

namespace Horn.Core.Spec.Dependencies
{
    public class When_We_Have_A_Single_Dependency : DirectorySpecificationBase
    {
        protected IBuildMetaData dependencyBuildMetaData;
        protected IBuildMetaData rootBuildMetaData;
        protected IDependencyTree dependencyTree;
        protected IPackageTree packageTree;
        protected IPackageTree dependentTree;

        protected override void Because()
        {
            rootBuildMetaData = CreateStub<IBuildMetaData>();
            dependencyBuildMetaData = CreateStub<IBuildMetaData>();

            rootBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "root.boo", Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            rootBuildMetaData.BuildEngine.Dependencies.Add(new Dependency("simpleDependency", "simpleDependency"));
            dependencyBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "simpleDependency", Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            packageTree = CreateStub<IPackageTree>();
            packageTree.Stub(x => x.Name).Return("root");
            packageTree.Stub(x => x.GetBuildMetaData("root")).Return(rootBuildMetaData);

            dependentTree = CreateStub<IPackageTree>();
            dependentTree.Stub(x => x.Name).Return("simpleDependency");
            dependentTree.Stub(x => x.GetBuildMetaData("simpleDependency")).Return(dependencyBuildMetaData);

            packageTree.Stub(x => x.RetrievePackage(new Dependency("dependency", "dependency"))).IgnoreArguments().Return(dependentTree);

            dependencyTree = new DependencyTree(packageTree);
        }

        [Fact]
        public void Then_The_Dependency_Is_Built_Before_The_Root()
        {
            var buildList = new List<IPackageTree>(dependencyTree.BuildList);

            Assert.Contains(packageTree, dependencyTree.BuildList);
            Assert.Contains(dependentTree, dependencyTree.BuildList);
            Assert.InRange(buildList.IndexOf(dependentTree), 0, buildList.IndexOf(packageTree));
        }

    }

    public class When_We_Have_A_Circular_Dependency : DirectorySpecificationBase
    {
        protected IBuildMetaData dependencyBuildMetaData;
        protected IBuildMetaData rootBuildMetaData;
        protected IDependencyTree dependencyTree;
        protected IPackageTree packageTree;
        protected IPackageTree dependentTree;

        protected override void Because()
        {
            rootBuildMetaData = CreateStub<IBuildMetaData>();
            dependencyBuildMetaData = CreateStub<IBuildMetaData>();

            var simpleDependency = new Dependency("simpleDependency", "simpleDependency.boo");
            var rootDependency = new Dependency("root", "root.boo");

            rootBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "root.boo", Horn.Core.Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            rootBuildMetaData.BuildEngine.Dependencies.Add(simpleDependency);
            dependencyBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "simpleDependency.boo", Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            dependencyBuildMetaData.BuildEngine.Dependencies.Add(rootDependency);

            packageTree = CreateStub<IPackageTree>();
            packageTree.Stub(x => x.Name).Return("root");
            packageTree.Stub(x => x.GetBuildMetaData("root")).Return(rootBuildMetaData);

            dependentTree = CreateStub<IPackageTree>();
            dependentTree.Stub(x => x.Name).Return("simpleDependency");
            dependentTree.Stub(x => x.GetBuildMetaData("simpleDependency")).Return(dependencyBuildMetaData);

            packageTree.Stub(x => x.RetrievePackage(simpleDependency)).Return(dependentTree);
            packageTree.Stub(x => x.RetrievePackage(rootDependency)).Return(packageTree);
            dependentTree.Stub(x => x.RetrievePackage(simpleDependency)).Return(dependentTree);
            dependentTree.Stub(x => x.RetrievePackage(rootDependency)).Return(packageTree);
        }

        [Fact]
        public void Then_An_Exception_Is_Raised()
        {
            Exception ex = Assert.Throws<CircularDependencyException>(() => new DependencyTree(packageTree));
            Assert.Equal("root is a dependent of itself", ex.Message);
        }
    }

    public class When_A_Dependency_Doesnt_Exist : DirectorySpecificationBase
    {
        protected IBuildMetaData dependencyBuildMetaData;
        protected IBuildMetaData rootBuildMetaData;
        protected IDependencyTree dependencyTree;
        protected IPackageTree packageTree;
        protected IPackageTree dependentTree;

        protected override void Because()
        {
            rootBuildMetaData = CreateStub<IBuildMetaData>();
            dependencyBuildMetaData = CreateStub<IBuildMetaData>();

            rootBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "root.boo", Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());
            rootBuildMetaData.BuildEngine.Dependencies.Add(new Dependency("simpleDependency", "simpleDependency"));
            dependencyBuildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), "simpleDependency", Utils.Framework.FrameworkVersion.FrameworkVersion35, CreateStub<IDependencyDispatcher>());

            packageTree = CreateStub<IPackageTree>();
            packageTree.Stub(x => x.Name).Return("root");
            packageTree.Stub(x => x.GetBuildMetaData("root")).Return(rootBuildMetaData);

            dependentTree = new NullPackageTree();

            packageTree.Stub(x => x.RetrievePackage(new Dependency("dependency", "dependency"))).IgnoreArguments().Return(dependentTree);
        }

        [Fact]
        public void Then_An_UnknownInstallPackageException_Is_Thrown()
        {
            Assert.Throws<UnknownInstallPackageException>(() => new DependencyTree(packageTree));
        }
    }
}