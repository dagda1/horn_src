using System;
using System.IO;
using System.Linq;
using System.Threading;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Utils;

namespace Horn.Spec.Framework.Stubs
{
    public class GitSourceControlDouble : GitSourceControl
    {
        private string revision;

        public override string Revision
        {
            get
            {
                return revision;
            }
        }

        protected override string CurrentRevisionNumber(FileSystemInfo destination)
        {
            revision = Guid.NewGuid().ToString();

            return revision;
        }

        protected override void SetMonitor(string destination)
        {
            downloadMonitor = new DefaultDownloadMonitor();
        }

        protected override Thread StartMonitoring()
        {
            Console.WriteLine("Source control download monitoring started.");

            return null;
        }

        public override string Update(IPackageTree packageTree, FileSystemInfo destination)
        {
            Console.WriteLine(string.Format("pulling {0} to {1}", packageTree.Name, destination.FullName));

            revision = Guid.NewGuid().ToString();

            return revision;
        }

        protected override void StopMonitoring(Thread thread)
        {
            Console.WriteLine("Source control download monitoring stopped.");
        }

        public GitSourceControlDouble(string url, IGitWorker gitWorker) : base(url, gitWorker)
        {
        }
    }
}