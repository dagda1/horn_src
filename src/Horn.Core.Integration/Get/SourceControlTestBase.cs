using System;
using System.IO;
using Horn.Core.PackageStructure;
using Horn.Framework.helpers;

namespace Horn.Core.Spec.Integration.Get
{
    public abstract class SourceControlTestBase : TestBase
    {
        protected IPackageTree packageTree;

        protected override void  Before_each_spec()
        {
            packageTree = new PackageTree(PackageTreeHelper.CreateEmptyDirectoryStructureForTesting(), null);
        }
    }
}