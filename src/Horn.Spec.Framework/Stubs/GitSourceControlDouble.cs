using System;
using System.Linq;
using System.Threading;
using Horn.Core.SCM;
using Horn.Core.Utils;

namespace Horn.Spec.Framework.Stubs
{
    public class GitSourceControlDouble : GitSourceControl
    {
        protected override string CurrentRevisionNumber()
        {
            return Guid.NewGuid().ToString();
        }

        protected override string RunGitCommand(string args)
        {
            foreach (var arg in args.Where(x => true))
            {
                Console.WriteLine(arg);
            }

            return string.Empty;
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

        protected override void StopMonitoring(Thread thread)
        {
            Console.WriteLine("Source control download monitoring stopped.");
        }

        public GitSourceControlDouble(string url, IEnvironmentVariable environmentVariable) : base(url, environmentVariable)
        {
        }
    }
}