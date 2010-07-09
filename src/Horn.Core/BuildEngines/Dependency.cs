namespace Horn.Core.BuildEngines
{
    public class Dependency
    {
        public string Library { get; private set; }

        public string PackageName { get; private set; }

        public string Version { get; private set; }

        public static Dependency Parse(string item)
        {
            var parts = item.Split('|');

            if(parts.Length == 2)
                return new Dependency(parts[0], parts[1]);

            return new Dependency(parts[0], parts[1], parts[2]);
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", Library, PackageName);
        }

        public Dependency(string package, string library)
        {
            PackageName = package;
            Library = library;
        }

        public Dependency(string package, string library, string version) : this(package, library)
        {
            Version = version;  
        }
    }
}