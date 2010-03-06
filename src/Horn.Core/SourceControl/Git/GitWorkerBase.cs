using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Horn.Core.BuildEngines;
using Horn.Core.Extensions;

namespace Horn.Core.SCM
{
	public abstract class GitWorkerBase
		: IGitWorker
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GitWorkerBase));

		public virtual void CheckoutNewBranch(DirectoryInfo workingDirectory, string branch, string startingPoint, bool track)
		{
			StringBuilder command = new StringBuilder();
			command.AppendFormat("checkout -b {0} ", branch);

			if (!string.IsNullOrEmpty(startingPoint))
			{
				if (track)
				{
					command.Append("--track ");
				}
				command.Append(startingPoint);
			}

			log.InfoFormat("Checking out new branch {0} ({1} '{2}')", branch, track ? "tracking" : "starting at", startingPoint);
			RunGitCommand(workingDirectory, command.ToString(), false);
		}

		public virtual void Checkout(DirectoryInfo workingDirectory, string branch)
		{
			// Already exists and is tracked. Just switch to it.
			string command = string.Format("checkout {0}", branch);
			log.InfoFormat("Checking out existing branch '{0}'", branch);
			RunGitCommand(workingDirectory, command, false);
		}

		public virtual void Clone(string source, DirectoryInfo workingDirectory)
		{
			if (string.IsNullOrEmpty(source))
			{
				throw new InvalidOperationException("No clone source defined");
			}

			string destinationName = workingDirectory.FullName;
			destinationName = destinationName.TrimEnd('\\', '/');
			string cloneCommand = string.Format("clone {0} {1}", source.QuotePath(), destinationName.QuotePath());
			RunGitCommand(workingDirectory, cloneCommand, false);

			RunGitCommand(workingDirectory, "fetch", false);
		}

		public virtual string GetCurrentBranch(DirectoryInfo workingDirectory)
		{
			string output = RunGitCommand(workingDirectory, "branch", false);

			string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			
			string currentBranch = lines
				.Where(l => l.StartsWith("*"))
				.Select(l => l.Trim(new[] { '*', ' ' }))
				.FirstOrDefault();

			if (string.IsNullOrEmpty(currentBranch))
			{
				throw new InvalidOperationException(string.Format("Could not find current branch in git output:\n{0}", output));
			}
			return currentBranch;
		}

		public virtual string GetCurrentCheckoutRevision(DirectoryInfo workingDirectory)
		{
			string output = RunGitCommand(workingDirectory, "log -g -1 HEAD --pretty=format:%H", false);
			return output;
		}

		public virtual IEnumerable<GitHead> ListTags(DirectoryInfo workingDirectory)
		{
			return GetHeads(workingDirectory)
				.Where(head => head.IsTag);
		}

		public virtual IEnumerable<GitHead> ListBranches(DirectoryInfo workingDirectory)
		{
			return GetHeads(workingDirectory)
				.Where(head => head.IsHead || head.IsRemote);
		}

		public virtual void Pull(DirectoryInfo workingDirectory)
		{
			RunGitCommand(workingDirectory, "pull -v", true);
			RunGitCommand(workingDirectory, "fetch", false);
		}

		protected virtual IEnumerable<GitHead> GetHeads(DirectoryInfo workingDirectory)
		{
			string output = RunGitCommand(workingDirectory, "show-ref --dereference", false);
			List<GitHead> result = GitHeadOutputParser.ProcessShowRefOutputForHeads(output);
			return result;
		}

		public virtual string RunGitCommand(DirectoryInfo workingDirectory, string arguments, bool displayOutput)
		{
			IProcess process = BuildGitCommandProcess(workingDirectory, arguments);

			StringBuilder result = new StringBuilder();

			while (true)
			{
				string line = process.GetLineOrOutput();

				if (line == null)
				{
					break;
				}

				result.AppendLine(line);
				if (displayOutput)
				{
					log.Info(line);
				}
			}
			process.WaitForExit();

			return result.ToString().TrimEnd('\r', '\n');
		}

		protected abstract IProcess BuildGitCommandProcess(DirectoryInfo workingDirectory, string arguments);
	}
}