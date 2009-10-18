using System.IO;
using System.Runtime.Serialization;
using Horn.Core.PackageStructure;

namespace Horn.Services.Core.Log
{
    [DataContract(Name = "PackageTreeLog", Namespace = "http://hornget.com/services")]
    public class PackageTreeLog
    {
        private readonly IPackageTree packageTree;

        [DataMember]
        public virtual DirectoryInfo Location { get; set; }

        public PackageTreeLog(IPackageTree packageTree)
        {
            this.packageTree = packageTree;

            Location = packageTree.CurrentDirectory;
        }
    }
}