using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.Utils.Framework;

namespace Horn.Spec.Framework.Stubs
{
    public class BuildEngineStub : BuildEngine
    {
        //public override BuildEngine Build(IProcessFactory processFactory, PackageStructure.IPackageTree packageTree)
        //{
        //    return this;
        //}

        protected override void CopyArtifactsToBuildDirectory(Horn.Core.PackageStructure.IPackageTree packageTree)
        {
            Console.WriteLine("Copying");
        }

        protected override void CopyDependenciesTo(Horn.Core.PackageStructure.IPackageTree packageTree)
        {
            Console.WriteLine(packageTree.Name);
        }

        protected override void CopyFileFromWorkingToResult(FileInfo file, string outputFile)
        {
            Console.WriteLine(string.Format("source = {0}", file.FullName));

            Console.WriteLine(string.Format("destination = {0}", outputFile));
        }

        protected override void ProcessBuild(Horn.Core.PackageStructure.IPackageTree packageTree, IProcessFactory processFactory, string pathToBuildTool, string cmdLineArguments)
        {
            Console.WriteLine("Processing Build");
        }

        public BuildEngineStub(IBuildTool buildTool, IDependencyDispatcher dependencyDispatcher, List<Dependency> dependencies)
            : base(buildTool, "somefile.build", FrameworkVersion.FrameworkVersion35, dependencyDispatcher)
        {
            Dependencies = dependencies;
        }

        public BuildEngineStub(IBuildTool buildTool, string buildFile, FrameworkVersion version, IDependencyDispatcher dependencyDispatcher)
            : base(buildTool, buildFile, version, dependencyDispatcher)
        {
        }
    }
}