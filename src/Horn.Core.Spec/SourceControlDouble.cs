using System;
using System.IO;
using System.Threading;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Framework.helpers;

namespace Horn.Core.Spec
{
    public class SourceControlDouble : SVNSourceControl
    {
        private FileInfo _tempFile;
        public bool ExportWasCalled;

        public bool FileWasDownloaded
        {
            get
            {
                return (_tempFile != null) ? _tempFile.Exists : false;
            }
        }

        public override string Revision
        {
            get
            {
                return long.MaxValue.ToString();
            }
        }

        public void Dispose()
        {
            if (_tempFile != null && _tempFile.Exists)
                _tempFile.Delete();
        }

        protected override Thread StartMonitoring()
        {
            Console.WriteLine("Source control download monitoring started.");

            return null;
        }

        protected override void StopMonitoring(Thread thread)
        {
            Console.WriteLine("Source control download monitoring stopped.");
        }

        protected override void Initialise(IPackageTree packageTree)
        {
            Console.WriteLine("In initialise");
        }

        protected override string Download(FileSystemInfo destination)
        {
            Console.WriteLine("In Download");

            if (!destination.Exists)
                ((DirectoryInfo)destination).Create();

            _tempFile = new FileInfo(Path.Combine(destination.FullName, "horn.boo"));

            FileHelper.CreateFileWithRandomData(_tempFile.FullName);

            ExportWasCalled = true;

            return long.MaxValue.ToString();
        }

        public SourceControlDouble(string url)
            : base(url)
        {
            ExportPath = string.Empty;
        }

    }

    public class SourceControlDoubleWithFakeFileSystem : SourceControlDouble
    {
        protected override void RecordCurrentRevision(IPackageTree tree, string revision)
        {
            Console.WriteLine(revision);
        }

        public SourceControlDoubleWithFakeFileSystem(string url)
            : base(url)
        {
        }
    }

    public class SourceControlDoubleWitholdRevision : SourceControlDouble
    {
        public override string Revision
        {
            get
            {
                return "0";
            }
        }

        public SourceControlDoubleWitholdRevision(string url)
            : base(url)
        {
        }
    }
}