using System;
using System.IO;

namespace Horn.Services.Core.Tests.Unit.Helpers
{
    public class FileSystemHelper
    {
        public static DirectoryInfo GetFakeDummyHornDirectory()
        {
            var hornDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".horn");

            return new DirectoryInfo(hornDirectoryPath);
        }
    }
}