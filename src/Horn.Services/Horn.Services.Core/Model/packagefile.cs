using System.IO;
using System.Runtime.Serialization;

namespace horn.services.core.Value
{
    [DataContract(Name = "PackageFile", Namespace = "http://hornget.com/services")]
    public class PackageFile
    {
        private FileInfo fileInfo;

        [DataMember(Order = 1)]
        public string Name { get; set; }

        public PackageFile(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;

            Name = fileInfo.Name;
        }
    }
}