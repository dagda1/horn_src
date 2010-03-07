using System;
using System.IO;
using System.Threading;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Framework.helpers;

namespace Horn.Spec.Framework.Stubs
{
    public class SourceControlDouble : SVNSourceControl
    {
        private FileInfo _tempFile;
        public bool CheckOutWasCalled;
        public bool ExportWasCalled;
        public bool UpdateWasCalled;

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

        public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
        {
            return PerformSCMOperation(destination, GetOperation.CheckOut);
        }

        public void Dispose()
        {
            if (_tempFile != null && _tempFile.Exists)
                _tempFile.Delete();
        }

        public override string Export(IPackageTree packageTree, FileSystemInfo destination)
        {
            return PerformSCMOperation(destination, GetOperation.Export);
        }

        public override string Update(IPackageTree packageTree, FileSystemInfo destination)
        {
            return PerformSCMOperation(destination, GetOperation.Update);
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

        private string PerformSCMOperation(FileSystemInfo destination, GetOperation getOperation)
        {           
            Console.WriteLine("In Download performing a {0}", getOperation);

            if (!destination.Exists)
                ((DirectoryInfo)destination).Create();

            _tempFile = new FileInfo(Path.Combine(destination.FullName, "horn.boo"));

            FileHelper.CreateFileWithRandomData(_tempFile.FullName);

            if (getOperation == GetOperation.CheckOut)
                CheckOutWasCalled = true;
            else if (getOperation == GetOperation.Update)
                UpdateWasCalled = true;
            else if (getOperation == GetOperation.Export)
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