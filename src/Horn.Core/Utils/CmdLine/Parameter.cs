namespace Horn.Core.Utils.CmdLine
{
    public class Parameter
    {
        private readonly bool requiresArgument;

        public string Key { get; private set; }

        public bool Reoccurs { get; private set; }

        public bool Required { get; private set; }

        public bool RequiresArgument
        {
            get
            {
                return requiresArgument;
            }
        }

        public Parameter(string key, bool required, bool requiresArgument, bool reoccurs)
        {
            Key = key;
            Required = required;
            this.requiresArgument = requiresArgument;
            Reoccurs = reoccurs;
        }
    }
}