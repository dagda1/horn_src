namespace Horn.Core.SCM
{
    public interface IDownloadMonitor
    {
        bool StopMonitoring { get; set; }

        void StartMonitoring();
    }
}