using Horn.Core.PackageStructure;

namespace Horn.Services.Core.Tests.Unit.Helpers
{
    public static class PackageTreeHelper
    {
        public static IPackageTree GetFakePackageTree()
        {
            return new PackageTree(FileSystemHelper.GetFakeDummyHornDirectory(), null);
        }
    }
}