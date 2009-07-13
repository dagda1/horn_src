using System;
using System.IO;
using Horn.Core.PackageStructure;
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

        public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
        {
            SvnUpdateResult result = null;

            using (var client = new SvnClient())
            {
                try
                {
                    client.CheckOut(new SvnUriTarget(new Uri(Url)), destination.FullName, out result);
                }
                catch (SvnRepositoryIOException sre)
                {
                    HandleExceptions(sre);
                }
                catch (SvnObstructedUpdateException sue)
                {
                    HandleExceptions(sue);
                }
            }

            return result.Revision.ToString();
        }

        public override string Export(IPackageTree packageTree, FileSystemInfo destination)
        {
            SvnUpdateResult result = null;

            using (var client = new SvnClient())
            {
                try
                {
                    client.Export(Url, destination.FullName, new SvnExportArgs { Overwrite = true }, out result);
                }
                catch (SvnRepositoryIOException sre)
                {
                    HandleExceptions(sre);
                }
                catch (SvnObstructedUpdateException sue)
                {
                    HandleExceptions(sue);
                }
            }

            return result.Revision.ToString();
        }

        public override string Update(IPackageTree packageTree, FileSystemInfo destination)
        {
            SvnUpdateResult result = null;

            using (var client = new SvnClient())
            {
                try
                {
                    client.Update(destination.FullName, out result);
                }
                catch (SvnRepositoryIOException sre)
                {
                    HandleExceptions(sre);
                }
                catch (SvnObstructedUpdateException sue)
                {
                    HandleExceptions(sue);
                }
            }

            return result.Revision.ToString();
        }

        protected override void Initialise(IPackageTree packageTree)
        {
            if(!packageTree.Root.Name.StartsWith(PackageTree.RootPackageTreeName))
                throw new InvalidOperationException("The root of the package tree is not named .horn");

            if (!packageTree.WorkingDirectory.Exists)
                return;
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