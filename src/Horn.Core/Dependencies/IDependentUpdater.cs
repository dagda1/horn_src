using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dependencies
{
    public interface IDependentUpdater
    {
        void Update(DependentUpdaterContext dependentUpdaterContext);
    }

    public class DependentUpdaterContext
    {

        private readonly IPackageTree package;
        private readonly IEnumerable<string> dependencyPaths;
        private readonly Dependency dependency;


        public Dependency Dependency
        {
            get { return dependency; }
        }

        public IEnumerable<string> DependencyPaths
        {
            get { return dependencyPaths; }
        }

        public DirectoryInfo WorkingDirectory
        {
            get { return package.WorkingDirectory; }
        }



        public DependentUpdaterContext(IPackageTree package, IEnumerable<string> dependencyPaths, Dependency dependency)
        {
            this.package = package;
            this.dependencyPaths = dependencyPaths;
            this.dependency = dependency;
        }



    }
}