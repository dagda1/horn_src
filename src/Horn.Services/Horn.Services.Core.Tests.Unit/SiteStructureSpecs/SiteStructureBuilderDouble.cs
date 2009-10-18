using System.IO;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Services.Core.Builder;
using horn.services.core.Value;

namespace Horn.Services.Core.Tests.Unit.SiteStructureSpecs
{
    public class SiteStructureBuilderDouble : SiteStructureBuilder
    {
        protected override void BuildAndZipPackage(IFileSystemProvider fileSystemProvider, Package package, DirectoryInfo newDirectory, DirectoryInfo tempDirectory)
        {
            var tempFileName = Path.Combine(newDirectory.FullName, string.Format("{0}.txt", package.FileName));
            fileSystemProvider.WriteTextFile(tempFileName, "some text");

            var zip = fileSystemProvider.ZipFolder(newDirectory, newDirectory, package.FileName);

            //fileSystemProvider.CopyFile(zip.FullName, zip.FullName, true);                

            try
            {
                fileSystemProvider.DeleteFile(tempFileName);
            }
            catch
            {
            }            
        }

        public SiteStructureBuilderDouble(IMetaDataSynchroniser metaDataSynchroniser, IFileSystemProvider fileSystemProvider, string dropDirectoryPath) : base(metaDataSynchroniser, fileSystemProvider, dropDirectoryPath)
        {
        }
    }
}