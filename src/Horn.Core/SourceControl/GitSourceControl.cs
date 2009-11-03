using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GitCommands;
using Horn.Core.BuildEngines;
using Horn.Core.exceptions;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;
using System.Linq;

namespace Horn.Core.SCM
{
    public class GitSourceControl : SourceControl
    {
        public override string Revision
        {
            get
            {
                //We always want to do a pull with git
                return Guid.NewGuid().ToString();
            }
        }

        public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;

            try
            {
                if (!destination.Exists)
                    Directory.CreateDirectory(destination.FullName);

                var result = RunGitCommand(GitCommands.GitCommands.CloneCmd(Url, destination.FullName, false).Replace("-v", ""));
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

        protected virtual string CurrentRevisionNumber()
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

        public override string Export(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;
            //nothing here for now.
            return CurrentRevisionNumber();
        }

        public override bool ShouldUpdate(string currentRevision, IPackageTree packageTree)
        {
            return true;
        }

        protected override void Initialise(IPackageTree packageTree)
        {
        }

        protected virtual string RunGitCommand(string args)
        {
            return GitCommands.GitCommands.RunCmd(string.Format("{0}git.exe", Settings.GitBinDir), args);
        }

        protected virtual void SetupGit(IEnvironmentVariable environmentVariable)
        {
            string gitDir = environmentVariable.GetDirectoryFor("git.cmd");

            if (string.IsNullOrEmpty(gitDir))
                throw new EnvironmentVariableNotFoundException("No environment variable found for the git.cmd file.");

            Settings.GitDir = gitDir;
            Settings.GitBinDir = Path.Combine(new DirectoryInfo(gitDir).Parent.FullName, "bin");
            Settings.UseFastChecks = false;
            Settings.ShowGitCommandLine = false;
        }

        public override string Update(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;

            try
            {
                var processFactory = new DiagnosticsProcessFactory();

                var git = Path.Combine(Settings.GitBinDir, "git.exe");

                IProcess process = processFactory.GetProcess(git, "pull -v ", packageTree.WorkingDirectory.FullName);

                while (true)
                {
                    string line = process.GetLineOrOutput();

                    if (line == null)
                        break;

                    log.Info(line);
                }

                try
                {
                    process.WaitForExit();
                }
                catch (ProcessFailedException)
                {
                    throw new GitPullFailedException(string.Format("A git pull failed for the {0} package", packageTree.Name));
                }

                //TODO: The following should work.  Might be the way I set up msysgit?
                //GitCommands.GitCommands.Pull("origin", "master", false);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

        public GitSourceControl(string url, IEnvironmentVariable environmentVariable) : base(url)
        {
            SetupGit(environmentVariable);
        }

        public GitSourceControl(IEnvironmentVariable environmentVariable)
        {
            SetupGit(environmentVariable);
        }
    }
}