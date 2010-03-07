using System;
using System.Collections.Generic;
using System.Linq;

namespace Horn.Core.SCM
{
	/// <summary>
	/// Logic lifted from GitCommands: http://github.com/spdr870/gitextensions
	/// </summary>
	public class GitHeadOutputParser
	{
		private static GitHead ProcessShowRefLineForHead(string line)
		{
			if (line.Length <= 42)
			{
				return null;
			}

			GitHead item = new GitHead
			{
				Guid = line.Substring(0, 40),
				Name = line.Substring(41).Trim()
			};
			if ((item.Name.Length > 0) && (item.Name.LastIndexOf("/") > 1))
			{
				if (item.Name.Contains("refs/tags/"))
				{
					if (item.Name.Contains("^{}"))
					{
						item.Name = item.Name.Substring(0, item.Name.Length - 3);
					}
					item.Name = item.Name.Substring(item.Name.LastIndexOf("/") + 1);
					item.IsHead = false;
					item.IsTag = true;
				}
				else
				{
					item.IsHead = item.Name.Contains("refs/heads/");
					item.IsRemote = item.Name.Contains("refs/remotes/");
					item.IsTag = false;
					item.IsOther = (!item.IsHead && !item.IsRemote) && !item.IsTag;
					if (item.IsHead)
					{
						item.Name = item.Name.Substring(item.Name.LastIndexOf("heads/") + 6);
					}
					else if (item.IsRemote)
					{
						item.Name = item.Name.Substring(item.Name.LastIndexOf("remotes/") + 8);
					}
					else
					{
						item.Name = item.Name.Substring(item.Name.LastIndexOf("/") + 1);
					}
				}
			}
			return item;
		}

		public static List<GitHead> ProcessShowRefOutputForHeads(string output)
		{
			string[] lines = output.Split('\n');

			var heads = lines
				.Select(l => ProcessShowRefLineForHead(l))
				.Where(head => head != null)
				.ToList();
			return heads;
		}
	}
}