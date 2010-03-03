namespace Horn.Core.Utils.CmdLine
{
    public class Parameter
    {
        private readonly bool requiresArgument;

        public string Key { get; private set; }

        public bool Reoccurs { get; private set; }

        public bool Required { get; private set; }

		public string[] SupersededBy { get; private set; }

        public bool RequiresArgument
        {
            get
            {
                return requiresArgument;
            }
        }

        public Parameter(string key, bool required, bool requiresArgument, bool reoccurs)
			: this(key, required, new string[0], requiresArgument, reoccurs)
        {
        }
		public Parameter(string key, bool required, string[] supersededBy, bool requiresArgument, bool reoccurs)
		{
			Key = key;
			Required = required;
			SupersededBy = supersededBy;
			this.requiresArgument = requiresArgument;
			Reoccurs = reoccurs;
		}
    }
}