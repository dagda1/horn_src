using System;
using Horn.Core.Dsl;
using horn.services.core.Value;

namespace Horn.Services.Core.Tests.Unit.Doubles
{
    public class PackageDouble : Package
    {
        public override void SetContents(System.IO.DirectoryInfo buildDirectory)
        {
            Console.WriteLine("In SetContents of PackageDouble");
        }

        public PackageDouble(Category parent, IBuildMetaData buildMetaData) : base(parent, buildMetaData)
        {
        }
    }
}