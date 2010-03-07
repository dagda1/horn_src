using System;
using System.IO;

namespace Horn.Core.exceptions
{
	public class GitCmdDirectoryNotFoundException
		: DirectoryNotFoundException
	{
		public GitCmdDirectoryNotFoundException(string message)
			: base(message)
		{
		}
	}
}