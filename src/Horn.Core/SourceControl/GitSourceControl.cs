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
        private string _branchName = "master";
        private IGitCommand _gitCommand = new CmdInvokedGitCommand();

        public override string Revision
        {
            get
            {
                //We always want to do a pull with git
                return Guid.NewGuid().ToString();
            }
        }

        public string BranchName
        {
            get { return _branchName; }
            set { _branchName = value; }
        }

        public IGitCommand GitCommand
        {
            get { return _gitCommand; }
            set { _gitCommand = value; }
        }

        public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
        {
            Settings.WorkingDir = destination.FullName;

            try
            {
                if (!destination.Exists)
                    Directory.CreateDirectory(destination.FullName);

                var result = _gitCommand.Run(GitCommands.GitCommands.CloneCmd(Url, destination.FullName, false, 1).Replace("-v", ""));

				if(BranchName != "master")
				{
					CreateAndTrackRemoteBranch(BranchName, destination);
				}
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

		private bool IsBranchCheckedOut(string branchName)
		{
			var branches = _gitCommand.Run("branch").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			var currentBranch = branches.Single(x => x.Trim().StartsWith("*"));
			return currentBranch.Trim(' ', '*') == branchName;
		}

		private bool BranchExists(string branchName)
		{
			var branches = GitCommands.GitCommands.GetHeads(false, true);
			return branches.Any(x => x.Name == branchName);
		}

    	private void CreateAndTrackRemoteBranch(string branchName, FileSystemInfo destination)
    	{
    		Settings.WorkingDir = destination.FullName;
			
    		//doesn't look like there's an equivalent for this in GitCommands
    		const string trackRemoteBranch = "checkout -b {0} --track origin/{0}";
    		string command = string.Format(trackRemoteBranch, branchName);

			log.Info("Tracking remote branch " + branchName);
			_gitCommand.Run(command);
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
                IProcess process = _gitCommand.Run("pull -v", packageTree.WorkingDirectory.FullName);

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

				//Ensure we're on the right branch
				if(! IsBranchCheckedOut(BranchName))
				{
					if(BranchExists(BranchName))
					{
						_gitCommand.Run(string.Format("checkout {0}", BranchName));
					}
					else
					{
						CreateAndTrackRemoteBranch(BranchName, destination);
					}
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