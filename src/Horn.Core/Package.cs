using Horn.Core.Dsl;

namespace Horn.Core
{
    public class Package
    {
        public IBuildMetaData BuildMetaData{ get; private set;}

        public string Name { get; private set; }

        public Package(string name, IBuildMetaData buildMetaData)
        {
            BuildMetaData = buildMetaData;

            Name = name;
        }
    }
}