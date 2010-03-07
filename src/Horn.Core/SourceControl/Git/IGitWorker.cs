using System;
using System.Collections.Generic;
using System.IO;

namespace Horn.Core.SCM
{
	public interface IGitWorker
	{
		void CheckoutNewBranch(DirectoryInfo workingDirectory, string branch, string startingPoint, bool track);
		void Checkout(DirectoryInfo workingDirectory, string branch);

		void Clone(string source, DirectoryInfo workingDirectory);

		string GetCurrentBranch(DirectoryInfo workingDirectory);
		string GetCurrentCheckoutRevision(DirectoryInfo workingDirectory);

		IEnumerable<GitHead> ListTags(DirectoryInfo workingDirectory);
		IEnumerable<GitHead> ListBranches(DirectoryInfo workingDirectory);

		void Pull(DirectoryInfo workingDirectory);
	}
}