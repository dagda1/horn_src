using System;
using System.IO;
using System.Linq;

using Horn.Core.exceptions;
using Horn.Core.PackageStructure;

namespace Horn.Core.SCM
{
	public class GitSourceControl : SourceControl
	{
		private string _branchName = "master";
		public string BranchName
		{
			get { return _branchName; }
			set { _branchName = value; }
		}

		private readonly IGitWorker _gitWorker;
		public IGitWorker GitWorker
		{
			get { return _gitWorker; }
		}

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
			try
			{
				if (!destination.Exists)
				{
					Directory.CreateDirectory(destination.FullName);
				}

				GitWorker.Clone(Url, (DirectoryInfo)destination);

				SwitchToBranchOrTag(BranchName, destination);
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}

			return CurrentRevisionNumber(destination);
		}

		protected virtual void SwitchToBranchOrTag(string name, FileSystemInfo destination)
		{
			DirectoryInfo workingDirectory = (DirectoryInfo)destination;
			string current = GitWorker.GetCurrentBranch((DirectoryInfo)destination);
			if (current == name)
			{
				return;
			}

			if (GitWorker.ListBranches(workingDirectory).Any(head => !head.IsRemote && head.Name == name))
			{
				GitWorker.Checkout(workingDirectory, name);
				return;
			}

			if (GitWorker.ListTags(workingDirectory).Any(head => head.Name == name))
			{
				GitWorker.CheckoutNewBranch(workingDirectory, name, name, true);
				return;
			}

			string expectedRemoteBranch = string.Format("origin/{0}", name);
			if (GitWorker.ListBranches(workingDirectory).Any(head => head.IsRemote && head.Name == expectedRemoteBranch))
			{
				GitWorker.CheckoutNewBranch(workingDirectory, name, expectedRemoteBranch, true);
				return;
			}

			throw new GitBranchNotFoundException(name);
		}

		protected virtual string CurrentRevisionNumber(FileSystemInfo destination)
		{
			string rev = null;

			try
			{
				rev = GitWorker.GetCurrentCheckoutRevision((DirectoryInfo)destination);
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}

			return rev;
		}

		public override string Export(IPackageTree packageTree, FileSystemInfo destination)
		{
			//nothing here for now.
			return CurrentRevisionNumber(destination);
		}

		public override bool ShouldUpdate(string currentRevision, IPackageTree packageTree)
		{
			return true;
		}

		protected override void Initialise(IPackageTree packageTree)
		{
		}

		public override string Update(IPackageTree packageTree, FileSystemInfo destination)
		{
			try
			{
				try
				{
					GitWorker.Pull((DirectoryInfo)destination);
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

			return CurrentRevisionNumber(destination);
		}

		public GitSourceControl(string url, IGitWorker gitWorker)
			: base(url)
		{
			_gitWorker = gitWorker;
		}

		public GitSourceControl(IGitWorker gitWorker)
		{
			_gitWorker = gitWorker;
		}
	}
}