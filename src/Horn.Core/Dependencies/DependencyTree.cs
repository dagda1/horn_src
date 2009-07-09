using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dependencies
{
    public class DependencyTree : IDependencyTree
    {
        private BuildTree buildTree;

        public IList<IPackageTree> BuildList
        {
            get
            {
                var buildList = buildTree.GetBuildList().OrderBy(x => x.GetBuildMetaData(x.Name).BuildEngine.Dependencies.Count).ToList();

                var parent = buildList.Where(x => x.Name.ToLower().Equals(PackageTree.Name.ToLower())).First();

                buildList.Remove(parent);

                buildList.Insert((buildList.Count), parent);

                return buildList;
            }
        }

        public IEnumerator<IPackageTree> GetEnumerator()
        {
            return BuildList.GetEnumerator();
        }

        private void CalculateDependencies(IPackageTree packageTree)
        {
            buildTree = CalculateDependencies(packageTree, null);
        }

        private BuildTree CalculateDependencies(IPackageTree packageTree, BuildTree currentTree)
        {
            if (currentTree == null)
            {
                currentTree = new BuildTree(packageTree);
            }
            else
            {
                if (HasACircularDependency(currentTree, packageTree))
                {
                    throw new CircularDependencyException(packageTree.Name);
                }
                
                InsertDependenciesBeforeParent(currentTree, packageTree);
            }

            var buildMetaData = packageTree.GetBuildMetaData(packageTree.Name);

            var buildEngine = buildMetaData.BuildEngine;

            var dependencies = buildEngine.Dependencies;

            foreach (var dependency in dependencies)
            {
                var package = packageTree.RetrievePackage(dependency.PackageName);

                CalculateDependencies(package, currentTree);
            }

            return currentTree;

        }

        private void InsertDependenciesBeforeParent(BuildTree tree, IPackageTree packageTree)
        {
            tree.AddChild(packageTree);
        }

        private bool HasACircularDependency(BuildTree tree, IPackageTree packageTree)
        {
            return tree.GetAncestors().Contains(packageTree);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DependencyTree(IPackageTree packageTree)
        {
            PackageTree = packageTree;

            CalculateDependencies(PackageTree);
        }

        private IPackageTree PackageTree { get; set; }
    }
}