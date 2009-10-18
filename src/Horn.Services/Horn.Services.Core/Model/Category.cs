using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Horn.Core.PackageStructure;
using Horn.Services.Core.Extensions;

namespace horn.services.core.Value
{
    [DataContract(Name = "Category", Namespace = "http://hornget.com/services")]
    public class Category : IResource
    {
        public bool IsRoot
        {
            get { return (Parent == null); }
        }

        [DataMember(Order = 1)]
        public string Name { get; private set; }

        [DataMember(Order = 2)]
        public List<Category> Categories { get; set; }

        [DataMember(Order = 3)]
        public List<Package> Packages { get; set; }

        public IResource Parent { get; private set; }

        [DataMember(Order = 4)]
        public string Url
        {
            get
            {
                return this.GetResourceUrl();
            }
            set
            {
                Console.WriteLine(value);
            }

        }

        public Category(Category parent, string name)
        {
            Categories = new List<Category>();

            Packages = new List<Package>();

            Name = name;

            Parent = parent;
        }


        public Category(Category parent, IPackageTree packageTreeNode)
        {
            Categories = new List<Category>();

            Packages = new List<Package>();

            Name = packageTreeNode.Name;

            Parent = parent;

            foreach (var buildMetaData in packageTreeNode.GetAllPackageMetaData())
            {
                if (buildMetaData.InstallName.IndexOf("mvccontrib") > -1)
                    Debugger.Break();

                Packages.Add(new Package(this, buildMetaData));
            }
        }
    }
}