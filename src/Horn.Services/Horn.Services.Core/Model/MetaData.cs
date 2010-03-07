using System.Runtime.Serialization;

namespace horn.services.core.Value
{
    [DataContract(Name = "MetaData", Namespace = "http://hornget.com/services")]
    public class MetaData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        public MetaData(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}