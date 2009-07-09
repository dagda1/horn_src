namespace Horn.Core.BuildEngines
{
    public class Dependency
    {
        public string Library { get; private set; }

        public string PackageName { get; private set; }

        public static Dependency Parse(string item)
        {
            return new Dependency(item.Split('|')[0], item.Split('|')[1]);
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
    }
}