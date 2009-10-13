using System;
using System.IO;
using GitCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;

namespace Horn.Core.SCM
{
    public class GitSourceControl : SourceControl
    {
        public GitSourceControl(IEnvironmentVariable environmentVariable)
        {
            SetupGit(environmentVariable);
        }

        private void SetupGit(IEnvironmentVariable environmentVariable)
        {
            string gitDir = environmentVariable.GetDirectoryFor("git.cmd");

            if (string.IsNullOrEmpty(gitDir))
                throw new EnvironmentVariableNotFoundException("No environment variable found for the git.cmd file.");

            Settings.GitDir = gitDir;
            Settings.GitBinDir = Path.Combine(new DirectoryInfo(gitDir).Parent.FullName, "bin");
            Settings.UseFastChecks = false;
            Settings.ShowGitCommandLine = false;
        }

        public override string Revision
        {
            get
            {
                try
                {
                    return GitCommands.GitCommands.GetRemoteHeads(Url, false, true)[0].Guid;
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return "1";
            }
        }

        private string CurrentRevisionNumber()
        {
            string rev = null;
            try
            {
                rev = GitCommands.GitCommands.GetCurrentCheckout();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return rev;
        }

        public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;

            try
            {
                if (!destination.Exists)
                    Directory.CreateDirectory(destination.FullName);
                RunGitCommand(GitCommands.GitCommands.CloneCmd(Url, destination.FullName, false).Replace("-v", ""));
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

        public override string Update(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;

            try
            {
                GitCommands.GitCommands.Pull("origin", "master", false);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

        public override string Export(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;
            //nothing here for now.
            return CurrentRevisionNumber();
        }

        private string RunGitCommand(string args)
        {
            return GitCommands.GitCommands.RunCmd(Settings.GitBinDir + "git.exe", args);
        }

        protected override void Initialise(IPackageTree packageTree)
        {
            //I don't know what we need this. it's taken from the svn one. (may)
            if (!packageTree.Root.Name.StartsWith(PackageTree.RootPackageTreeName))
                throw new InvalidOperationException("The root of the package tree is not named .horn");

            if (!packageTree.WorkingDirectory.Exists)
                return;
        }

        public override bool ShouldUpdate(string currentRevision)
        {
            string revision = Revision;
            log.InfoFormat("Current Revision is = {0}", currentRevision);

            log.InfoFormat("Revision at remote scm is {0}", revision);

            return Revision != currentRevision;
        }
    }
}