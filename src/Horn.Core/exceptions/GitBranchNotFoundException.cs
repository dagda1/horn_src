using System;

namespace Horn.Core.exceptions
{
	public class GitBranchNotFoundException
		: InvalidOperationException
	{
		public GitBranchNotFoundException(string branch)
			: base(GenerateMessage(branch))
		{
		}

		public GitBranchNotFoundException(string branch, Exception innerException)
			: base(GenerateMessage(branch), innerException)
		{
		}

		private static string GenerateMessage(string branch)
		{
			return string.Format("Branch '{0}' not found", branch);
		}
	}
}