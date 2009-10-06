using System;
using System.Collections.Generic;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Dsl;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Spec.BuildEngineSpecs;
using Horn.Core.Spec.Doubles;
using Horn.Framework.helpers;
using Horn.Spec.Framework.Stubs;
using Rhino.Mocks;

namespace Horn.Core.Spec.helpers
{
    public static class TreeHelper
    {
        public static IPackageTree GetTempEmptyPackageTree()
        {
            var treeDirectory = PackageTreeHelper.CreateEmptyDirectoryStructureForTesting();

            return new PackageTree(treeDirectory, null);            
        }

        public static IPackageTree GetTempPackageTree()
        {
            var treeDirectory =  PackageTreeHelper.CreateDirectoryStructureForTesting();

            return new PackageTree(treeDirectory, null);
        }

        public static IBuildMetaData GetPackageTreeParts(List<Dependency> dependencies)
        {
            var buildEngine = new BuildEngineStub(new BuildToolStub(), null, dependencies);
            var sourceControl = new SourceControlDouble("http://someurl.com");
            return new BuildMetaDataStub(buildEngine, sourceControl);
        }

        public static IBuildMetaData GetPackageTreeParts(List<Dependency> dependencies, List<string> cmds )
        {
            var buildEngine = new BuildEngineStub(new BuildToolStub(), null, dependencies);
            var sourceControl = new SourceControlDouble("http://someurl.com");
            var buildMetaData = new BuildMetaDataStub(buildEngine, sourceControl);

            buildMetaData.PrebuildCommandList.AddRange(cmds);

            return buildMetaData;
        }

        public static IBuildMetaData GetPackageTreeParts(List<Dependency> dependencies, List<SourceControl> exportList)
        {
            var buildEngine = new BuildEngineStub(new BuildToolStub(), null, dependencies);
            var buildMetaData = new BuildMetaDataStub(buildEngine, null);

            buildMetaData.ExportList.AddRange(exportList);

            return buildMetaData;
        }

        public static IBuildMetaData GetPackageTreeParts(List<Dependency> dependencies, List<IRepositoryElement> repositoryElements)
        {
            var buildEngine = new BuildEngineStub(new BuildToolStub(), null, dependencies);
            var buildMetaData = new BuildMetaDataStub(buildEngine, null);

            buildMetaData.RepositoryElementList.AddRange(repositoryElements);

            return buildMetaData;
        }

        public static IPackageTree CreatePackageTreeNode(string packageName, string[] dependencyNames)
        {
            var buildMetaData = MockRepository.GenerateStub<IBuildMetaData>();
            buildMetaData.BuildEngine = new BuildEngine(new BuildToolStub(), String.Format("{0}.boo", packageName), Utils.Framework.FrameworkVersion.FrameworkVersion35, MockRepository.GenerateStub<IDependencyDispatcher>());
            foreach (string dependencyName in dependencyNames)
            {
                var dependency = new Dependency(dependencyName, dependencyName);

                buildMetaData.BuildEngine.Dependencies.Add(dependency);
            }

            var packageTree = MockRepository.GenerateStub<IPackageTree>();
            packageTree.Stub(x => x.Name).Return(packageName);
            packageTree.Stub(x => x.GetBuildMetaData("root")).Return(buildMetaData).Repeat.Any();
            packageTree.Stub(x => x.GetBuildMetaData("complexDependency")).Return(buildMetaData).Repeat.Any();
            packageTree.Stub(x => x.GetBuildMetaData("sharedDependency")).Return(buildMetaData).Repeat.Any();

            return packageTree; 
        }
    }
}