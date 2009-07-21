using System.IO;
using Horn.Core.extensions;
using Horn.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.Extensions
{
    public class When_Copying_From_A_Folder : Specification
    {
        private DirectoryInfo source;
        private DirectoryInfo destination;

        protected override void Because()
        {
            source = new DirectoryInfo(DirectoryHelper.GetBaseDirectory().RemoveDebugFolderParts()).Parent;

            destination = new DirectoryInfo(Path.Combine(new DirectoryInfo("C:\\").FullName, "Working"));

            source.CopyToDirectory(destination, true);
        }

        //[Fact], too slow to be useful
        public void Then_SubFolders_And_Files_Are_Copied()
        {
            Assert.True(destination.GetDirectories().Length > 0);

            Assert.True(destination.GetFiles().Length > 0);            
        }
    }
}