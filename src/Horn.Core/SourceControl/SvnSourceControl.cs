using System;
using System.IO;
using System.Threading;
using Horn.Core.PackageStructure;
using log4net;
using SharpSvn;

namespace Horn.Core.SCM
{
    public class SVNSourceControl : SourceControl
    {
        public override string Revision
        {
            get
            {
                SvnInfoEventArgs info = null;

                using (var client = new SvnClient())
                {
                    try
                    {
                        client.GetInfo(SvnTarget.FromUri(new Uri(Url)), out info);
                    }
                    catch (SvnRepositoryIOException sre)
                    {
                        HandleExceptions(sre);

                        throw;
                    }
                    catch (SvnObstructedUpdateException sue)
                    {
                        HandleExceptions(sue);
                    }
                }

                return info.Revision.ToString();
            }
        }

        protected override void Initialise(IPackageTree packageTree)
        {
            if(!packageTree.Root.Name.StartsWith(PackageTree.RootPackageTreeName))
                throw new InvalidOperationException("The root of the package tree is not named .horn");

            if (!packageTree.WorkingDirectory.Exists)
                return;

            try
            {
                if (packageTree.Name != PackageTree.RootPackageTreeName)
                    packageTree.WorkingDirectory.Delete(true);
            }
            catch (IOException)
            {
                throw new IOException(string.Format("The horn process is trying to delete a working directory.  Please ensure you have no applications open in the {0} directory.", packageTree.Root.CurrentDirectory.FullName));
            }
        }

        protected override string Download(FileSystemInfo destination)
        {
            SvnUpdateResult result = null;

            using (var client = new SvnClient())
            {
                try
                {
                    var args = new SvnExportArgs {Overwrite = true};

                    client.Export(Url, destination.FullName, args, out result);
                }
                catch (SvnRepositoryIOException sre)
                {
                    HandleExceptions(sre);

                    throw;
                }
                catch (SvnObstructedUpdateException sue)
                {
                    HandleExceptions(sue);
                }
                catch(Exception ex)
                {
                    HandleExceptions(ex);

                    throw;
                }
            }

            return result.Revision.ToString();
        }

        protected override void SetMonitor(string destination)
        {
            downloadMonitor = new DownloadMonitor(destination);
        }

        public SVNSourceControl()
        {
        }

        public SVNSourceControl(string url)
            : base(url)
        {
        }

        public SVNSourceControl(string url, string exportPath)
            : base(url, exportPath)
        {
        }
    }
}