using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Horn.Core.extensions;
using Horn.Core.PackageStructure;
using log4net;
using SharpSvn;

namespace Horn.Core.SCM
{
    public enum GetOperation
    {
        CheckOut,
        Update,
        Export
    }

    public abstract class SourceControl
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(SVNSourceControl));
        private static readonly Dictionary<string, string> downloadedPackages = new Dictionary<string, string>();
        private static readonly object locker = new object();
        protected  IDownloadMonitor downloadMonitor;

        public IDownloadMonitor DownloadMonitor
        {
            get { return downloadMonitor; }
        }

        public virtual string ExportPath { get; protected set; }

        public abstract string Revision { get; }

        public string Url {get; private set;}

        public abstract string CheckOut(IPackageTree packageTree, FileSystemInfo destination);

        public abstract string Export(IPackageTree packageTree, FileSystemInfo destination);

        public abstract string Update(IPackageTree packageTree, FileSystemInfo destination);

        protected abstract void Initialise(IPackageTree packageTree);

        public static void ClearDownLoadedPackages()
        {
            downloadedPackages.Clear();
        }

        public static T Create<T>(string url) where T : SourceControl
        {
            var sourceControl = IoC.Resolve<T>();

            sourceControl.Url = url;

            return sourceControl;
        }

        public virtual void RetrieveSource(IPackageTree packageTree)
        {
            if (downloadedPackages.ContainsKey(packageTree.Name))
                return;

            if (!packageTree.GetRevisionData().ShouldUpdate(new RevisionData(Revision)))
            {
                downloadedPackages.Add(packageTree.Name, packageTree.Name);

                return;
            }

            var revisionData = packageTree.GetRevisionData();
                
            Initialise(packageTree);

            SetMonitor(packageTree.WorkingDirectory.FullName);
            
            Thread monitoringThread = StartMonitoring();

            var revision = Download(packageTree, packageTree.WorkingDirectory, revisionData.Operation());

            StopMonitoring(monitoringThread);

            RecordCurrentRevision(packageTree, revision);
        }

        public virtual void RetrieveSource(IPackageTree packageTree, string path, bool initialise)
        {
            lock (locker)
            {
                if (initialise)
                    Initialise(packageTree);

                downloadMonitor = new DefaultDownloadMonitor();

                var fullPath = Path.Combine(packageTree.WorkingDirectory.FullName, path);

                FileSystemInfo exportPath = GetExportPath(fullPath);

                Thread monitoringThread = StartMonitoring();

                Download(packageTree, exportPath, GetOperation.Export);

                StopMonitoring(monitoringThread);
            }
        }

        protected virtual FileSystemInfo GetExportPath(string fullPath)
        {
            return FileSystemInfoExtensions.GetExportPath(fullPath);
        }

        protected void HandleExceptions(Exception ex)
        {
            log.Error(ex);

            if(downloadMonitor != null)
                downloadMonitor.StopMonitoring = true;

            throw new RemoteScmException(ex.UnwrapException());
        }

        protected virtual void RecordCurrentRevision(IPackageTree tree, string revision)
        {
            tree.GetRevisionData().RecordRevision(tree, revision);
        }

        protected virtual void StopMonitoring(Thread thread)
        {
            downloadMonitor.StopMonitoring = true;

            thread.Join();
        }

        protected virtual Thread StartMonitoring()
        {
            var thread = new Thread(downloadMonitor.StartMonitoring);

            thread.Start();

            return thread;
        }

        protected virtual void SetMonitor(string destination)
        {
            downloadMonitor = new DefaultDownloadMonitor();
        }

        protected virtual string Download(IPackageTree packageTree, FileSystemInfo destination, GetOperation operation)
        {
            string result = string.Empty;

            switch (operation)
            {
                case GetOperation.CheckOut:
                    result = CheckOut(packageTree, destination);
                    break;
                case GetOperation.Update:
                    result = Update(packageTree, destination);
                    break;
                case GetOperation.Export:
                    result = Export(packageTree, destination);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown get operation passed to Download {0}", operation));
            }

            return result;
        }

        protected SourceControl(string url)
        {
            Url = url;
        }

        protected SourceControl(string url, string exportPath)
        {
            Url = url;
            ExportPath = (string.IsNullOrEmpty(exportPath) ? "" : exportPath);
        }

        protected SourceControl()
        {
        }
    }
}