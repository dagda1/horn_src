using System.IO;
using Horn.Core.PackageStructure;
using Horn.Framework.helpers;

namespace Horn.Core.Spec
{
    public abstract class DirectorySpecificationBase : Specification
    {

        protected DirectoryInfo rootDirectory;


        protected override void Before_each_spec()
        {
            rootDirectory = PackageTreeHelper.CreateDirectoryStructureForTesting();
        }



    }
}