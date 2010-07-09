using System;
using System.IO;
using Horn.Core.Utils;
using Horn.Framework.helpers;
using Xunit;

namespace Horn.Core.Spec.Integration.Utils
{
    public class FileSystemProviderSpec : IDisposable
    {
        readonly string path = Path.Combine(DirectoryHelper.GetBaseDirectory(), DateTime.Now.Ticks.ToString());

        public void Dispose()
        {
            Directory.Delete(path);
        }

        private IFileSystemProvider CreateSUT()
        {
            return new FileSystemProvider();
        }

        [Fact]
        public void CreateDirectory_Will_Create_Directory()
        {
            CreateSUT().CreateDirectory(path);
            
            Assert.True(Directory.Exists(path));
        }
    }
}