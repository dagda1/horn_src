using System;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.PackageStructure;
using Horn.Core.Utils;

namespace Horn.Core.SCM
{
	public class MercurialSourceControl : SourceControl
	{
		private readonly IShellRunner shellRunner;
		private string hgDirectory;

		public MercurialSourceControl(IShellRunner shellRunner, IEnvironmentVariable environmentVariable)
		{
			this.shellRunner = shellRunner;
			hgDirectory = environmentVariable.GetDirectoryFor("hg.exe");
		}

		public override string Revision
		{
			get
			{
				string rev = null;
				try
				{
					rev = ParseRevision(shellRunner.RunCommand(GetHGExecutableLocation(), "id -i -r tip " + Url));
				}
				catch (Exception ex)
				{
					HandleExceptions(ex);
				}
				return rev;
			}
		}

		public string BranchName { get; set; }

		public override string CheckOut(IPackageTree packageTree, FileSystemInfo destination)
		{
			try
			{
				if (!destination.Exists)
					Directory.CreateDirectory(destination.FullName);
				RunHGCommand(string.Format("{0} {1} {2}", "clone", Url, destination.FullName), destination.FullName);
				UpdateBranch(destination);				
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}

			return CurrentRevisionNumber(destination.FullName);
		}

		public override string Export(IPackageTree packageTree, FileSystemInfo destination)
		{
			throw new NotImplementedException();
		}

		public override string Update(IPackageTree packageTree, FileSystemInfo destination)
		{
			try
			{
				RunHGCommand("pull", destination.FullName);
				UpdateBranch(destination);
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}

			return CurrentRevisionNumber(destination.FullName);
		}

		private void UpdateBranch(FileSystemInfo destination)
		{
			if (string.IsNullOrEmpty(BranchName))
				BranchName = "default";			
			RunHGCommand(string.Format("update -C \"{0}\"", BranchName), destination.FullName);			
		}

		protected override void Initialise(IPackageTree packageTree)
		{
			if (!packageTree.Root.Name.StartsWith(PackageTree.RootPackageTreeName))
				throw new InvalidOperationException("The root of the package tree is not named .horn");

			if (!packageTree.WorkingDirectory.Exists)
				return;
		}

		private string RunHGCommand(string args, string workingDirectory)
		{
			return shellRunner.RunCommand(GetHGExecutableLocation(), args, workingDirectory);
		}

		private string GetHGExecutableLocation()
		{
			return Path.Combine(hgDirectory, "hg.exe");
		}

		private string CurrentRevisionNumber(string destination)
		{
			string rev = null;
			try
			{
				rev = ParseRevision(RunHGCommand("id", destination));
			}
			catch (Exception ex)
			{
				HandleExceptions(ex);
			}
			return rev;
		}

		private string ParseRevision(string hgRevInfo)
		{
			if (hgRevInfo.Contains(" "))
			{
				return hgRevInfo.Split(' ')[0].Replace("+", "");
			}
			return hgRevInfo;
		}
	}
}