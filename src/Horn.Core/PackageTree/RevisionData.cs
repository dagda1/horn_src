using System;
using System.IO;
using System.Text;
using Horn.Core.SCM;
using log4net;

namespace Horn.Core.PackageStructure
{
	public class RevisionData : IRevisionData
	{
		private readonly FileInfo revisionFileInfo;
		private string revision;
		private static readonly ILog log = LogManager.GetLogger(typeof(RevisionData));
		public const string FileName = "revision.horn";
		public const string VersionedFileName = "revision-{0}.horn";

		public bool Exists
		{
			get { return revisionFileInfo.Exists; }
		}

		public string Revision
		{
			get
			{
				if (!string.IsNullOrEmpty(revision))
					return revision;

				try
				{
					using (var stream = revisionFileInfo.OpenRead())
					{
						var b = new byte[1024];
						var temp = new UTF8Encoding(true);

						while (stream.Read(b, 0, b.Length) > 0)
						{
							revision = temp.GetString(b).Trim(new[] { '\r', '\n', '\0' }).Split('=')[1];
						}

					}
				}
				catch (IOException ioe)
				{
					log.Error(ioe);

					return "0";
				}

				return revision;
			}
		}

		public virtual GetOperation Operation()
		{
			return ShouldCheckOut() ? GetOperation.CheckOut : GetOperation.Update;
		}

		public virtual void RecordRevision(IPackageTree packageTree, string revisionVlaue)
		{
			var fileInfo = GetRevisionFile(packageTree);

			RecordRevision(fileInfo, revisionVlaue);
		}

		public bool ShouldCheckOut()
		{
			return (Revision.Trim() == "0");
		}

		public bool ShouldUpdate(IRevisionData other)
		{
			log.InfoFormat("Current Revision is = {0}", Revision);

			log.InfoFormat("Revision at remote scm is {0}", other.Revision);

			return other.Revision != Revision;
		}

		private void RecordRevision(FileInfo fileInfo, string revisionValue)
		{
			File.WriteAllText(fileInfo.FullName, string.Format("revision={0}", revisionValue), Encoding.UTF8);
		}

		private FileInfo GetRevisionFile(IPackageTree packageTree)
		{
			string fileName;

			if (!packageTree.IsAversionRequest)
				fileName = Path.Combine(packageTree.CurrentDirectory.FullName, FileName);
			else
				fileName = Path.Combine(packageTree.CurrentDirectory.FullName,
										string.Format(VersionedFileName, packageTree.Version));

			return new FileInfo(fileName);
		}

		public RevisionData(string revision)
		{
			this.revision = revision;
		}

		public RevisionData(IPackageTree packageTree)
		{
			log.InfoFormat("Reading the current revision for {0}", packageTree.Name);

			revisionFileInfo = GetRevisionFile(packageTree);

			if (revisionFileInfo.Exists)
				return;

			RecordRevision(revisionFileInfo, "0");
		}
	}
}