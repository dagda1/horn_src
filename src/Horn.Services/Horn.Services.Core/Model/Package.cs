using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Horn.Core.Dsl;
using Horn.Services.Core.Extensions;

namespace horn.services.core.Value
{
    [DataContract(Name = "Package", Namespace = "http://hornget.com/services")]
    public class Package : IResource
    {
        public bool IsRoot
        {
            get { return false; }
        }

        public string FileName
        {
            get
            {
                return string.Format("{0}-{1}", Name, Version);
            }
        }

        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public List<MetaData> MetaData { get; set; }

        public IResource Parent { get; private set; }

        [DataMember(Order = 3)]
        public string Version { get; set; }

        [DataMember(Order = 4)]
        public string Url
        {
            get
            {
                var url = this.GetResourceUrl().Trim(new[]{'/'});

                return string.Format("{0}-{1}", url, Version);
            }
            set
            {
                Console.WriteLine(value);
            }
        }

        public bool IsTrunk
        {
            get { return Version == "trunk"; }
        }

        public Package(Category parent, IBuildMetaData buildMetaData)
        {
            Name = parent.Name;

            Version = buildMetaData.Version;

            MetaData = new List<MetaData>();

            Parent = parent;

            foreach (var projectInfo in buildMetaData.ProjectInfo)
            {
                MetaData.Add(new MetaData(projectInfo.Key, projectInfo.Value.ToString()));   
            }
        }
    }
}