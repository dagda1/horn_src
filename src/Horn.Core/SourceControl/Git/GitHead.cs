namespace Horn.Core.SCM
{
	public class GitHead
	{
		public string Guid { get; set; }
		public string Name { get; set; }
		public bool IsHead { get; set; }
		public bool IsOther { get; set; }
		public bool IsRemote { get; set; }
		public bool IsTag { get; set; }
	}
}