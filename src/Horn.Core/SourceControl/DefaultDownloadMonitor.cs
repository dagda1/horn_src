using System.Threading;
using log4net;

namespace Horn.Core.SCM
{
    public class DefaultDownloadMonitor : IDownloadMonitor
    {
        public static readonly ILog log = LogManager.GetLogger(typeof (DefaultDownloadMonitor));

        public bool StopMonitoring { get; set; }

        public void StartMonitoring()
        {
            while(!StopMonitoring)
            {
                log.Info("working......");

                Thread.Sleep(3000);
            }
        }
    }
}