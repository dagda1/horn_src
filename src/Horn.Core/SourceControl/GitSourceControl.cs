using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using GitCommands;

using Horn.Core.BuildEngines;
using Horn.Core.exceptions;
using Horn.Core.PackageStructure;

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

				SwitchToBranchOrTag(BranchName, destination);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }

		protected virtual string[] ListTags(FileSystemInfo destination)
		{
			Settings.WorkingDir = destination.FullName;
			var result = GitCommands.GitCommands.GetHeads(true, false)
				.Select(head => head.Name)
				.ToArray();
			return result;
		}

		protected virtual string[] ListBranches(FileSystemInfo destination, bool includeRemote)
		{
			Settings.WorkingDir = destination.FullName;
			var result = GitCommands.GitCommands.GetHeads(false, true)
				.Select(head => head.Name)
				.ToArray();
			return result;
		}

		protected virtual string GetCurrentBranch(FileSystemInfo destination)
		{
			return GitCommands.GitCommands.GetSelectedBranch();
		}

		protected virtual void SwitchToBranchOrTag(string name, FileSystemInfo destination)
		{
			string current = GetCurrentBranch(destination);
			if (current == name)
			{
				return;
			}

			if (ListBranches(destination, false).Contains(name))
			{
				// Already exists and is tracked. Just switch to it.
				string command = string.Format("checkout {0}", name);
				log.InfoFormat("Checking out existing branch '{0}'", name);
				_gitCommand.Run(command);
				return;
			}

			if (ListTags(destination).Contains(name))
			{
				// Isn't going to be tracked. Just switch to it.
				string command = string.Format("checkout -b {0} --track {0}", name);
				log.InfoFormat("Checking out new branch {0} (tracking tag '{0}')", name);
				_gitCommand.Run(command);
				return;
			}

			string expectedRemoteBranch = string.Format("origin/{0}", name);
			if (ListBranches(destination, true).Contains(expectedRemoteBranch))
			{
				// Create a local branch and track remote
				string command = string.Format("checkout -b {0} --track {1}", name, expectedRemoteBranch);
				log.InfoFormat("Checking out new branch {0} (tracking {1})", name, expectedRemoteBranch);
				_gitCommand.Run(command);
				return;
			}

			throw new GitBranchNotFoundException(name);
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

        protected virtual void SetupGit(string gitBinDirectory)
        {
			Settings.GitDir = FindGitCmdDirectory(gitBinDirectory);
            Settings.GitBinDir = gitBinDirectory;
            Settings.UseFastChecks = false;
            Settings.ShowGitCommandLine = false;
        }

		protected virtual string FindGitCmdDirectory(string gitBinDirectory)
		{
			// Look for the directory with git.cmd in
			string[] expectedRelativeLocations = new[]
			{
				"..",
				".",
				Path.Combine("..", "cmd"),
			};

			List<string> checkedLocations = new List<string>();

			foreach (var expectedLocation in expectedRelativeLocations)
			{
				string relative = Path.Combine(gitBinDirectory, expectedLocation);
				relative = Path.GetFullPath(relative);

				// The first value is the default if it fails to find git.cmd
				
				checkedLocations.Add(relative);

				string gitCmd = Path.Combine(relative, "git.cmd");
				if (File.Exists(gitCmd))
				{
					return relative;
				}
			}

			StringBuilder message = new StringBuilder();
			message.AppendFormat("Could not find the directory containing 'git.cmd' relative to '{0}'", gitBinDirectory)
				.AppendLine();
			message.AppendLine("Searched locations:");
			foreach (var location in checkedLocations)
			{
				message.AppendFormat("\t{0}", location)
					.AppendLine();
			}

			throw new GitCmdDirectoryNotFoundException(message.ToString());
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

				SwitchToBranchOrTag(BranchName, destination);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }

            return CurrentRevisionNumber();
        }
    
        public GitSourceControl(string url, string gitBinDirectory) : base(url)
        {
            SetupGit(gitBinDirectory);
        }

        public GitSourceControl(string gitBinDirectory)
        {
            SetupGit(gitBinDirectory);
        }
    }
}