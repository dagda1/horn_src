using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Core.PackageStructure;
using Horn.Spec.Framework.Stubs;

namespace Horn.Core.Spec.Dependencies
{
    public abstract class dependency_dispatcher_context : Specification
    {
        protected DependencyDispatcher dispatcher;
        protected IPackageTree packageTree;
        protected string targetDirectory;
        private string outputPath;
        protected string workingPath;
        protected string dependencyPath;
        protected Dependency[] dependencies;
        protected IDependentUpdaterExecutor dependentUpdater;
        protected List<string> directoriesToDelete = new List<string>();


        protected override void Before_each_spec()
        { 
            targetDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());

            packageTree = CreateStub<PackageTreeStub>(new[] { targetDirectory });
            dependentUpdater = CreateStub<IDependentUpdaterExecutor>();

            dependencies = new[] { new Dependency("Test", "Test.Dependency"), };

            outputPath = packageTree.OutputDirectory.FullName;
            ((PackageTreeStub)packageTree).Result = new DirectoryInfo(outputPath);
            workingPath = packageTree.WorkingDirectory.FullName;
            dependencyPath = Path.Combine(workingPath, "dependencies");

            CreateDirectories();
            CreateFiles();

            dispatcher = new DependencyDispatcher(dependentUpdater);
        }

        protected override void After_each_spec()
        {
            try
            {
                if (Directory.Exists(targetDirectory))
                    Directory.Delete(targetDirectory, true);
            }
            catch
            {                
            }
        }

        private void CreateFiles()
        {
            foreach (var dependency in dependencies)
            {
                File.WriteAllText(Path.Combine(outputPath, dependency.Library + ".dll"), "This is a fake dependency");
                File.WriteAllText(Path.Combine(dependencyPath, dependency.Library + ".dll"), "This is a fake dependency that will be replaced");
            }
        }

        private void CreateDirectories()
        {
            Directory.CreateDirectory(targetDirectory);
            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(workingPath);
            Directory.CreateDirectory(dependencyPath);
        }
    }
}