using System.IO;
using System.Threading;
using log4net;

namespace Horn.Core.SCM
{
    public class DownloadMonitor : IDownloadMonitor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (DownloadMonitor));
        private readonly string downloadDirectory;
        private FileSystemWatcher watcher;
        private bool _stopMonitoring;
        
        public bool StopMonitoring
        {
            get { return _stopMonitoring; }
            set 
            { 
                _stopMonitoring = value; 

                if(_stopMonitoring)
                    watcher.Dispose();
            }
        }

        public void StartMonitoring()
        {
            StopMonitoring = false;

            while ((!Directory.Exists(downloadDirectory) && (!StopMonitoring)))
            {
                Thread.Sleep(10);
            }

            watcher = new FileSystemWatcher(downloadDirectory)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.FullPath.Length > 240)
                return;

            var file = Path.GetFileName(e.FullPath);

            var dir = Path.GetDirectoryName(e.FullPath);

            if (!string.IsNullOrEmpty(file) && (file.IndexOf("tmp") > -1))
                return;

            if (!string.IsNullOrEmpty(dir) && (dir.IndexOf("tmp") > -1))
                return;

            log.InfoFormat("{0} was {1} in {2}", file, e.ChangeType, dir);
        }

        public DownloadMonitor(string downloadDirectory)
        {
            this.downloadDirectory = downloadDirectory;
        }
    }
}