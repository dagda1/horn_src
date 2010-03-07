using System.Collections.Generic;

namespace Horn.Core.PackageStructure
{
    public static class PackageTreeExtensions
    {
        public static IEnumerable<IPackageTree> GetAllPackages(this IPackageTree parent)
        {
            foreach (var child in parent.Children)
            {
                yield return child;

                foreach (var descendant in child.GetAllPackages())
                {
                    yield return descendant;
                }
            }
        }

    }
}