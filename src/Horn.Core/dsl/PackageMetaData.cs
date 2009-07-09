using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;

namespace Horn.Core.Dsl
{
    public class PackageMetaData : IQuackFu
    {
        public Dictionary<string, object> PackageInfo { get; set; }

        public object QuackGet(string name, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public object QuackInvoke(string name, params object[] args)
        {
            throw new NotImplementedException();
        }

        public object QuackSet(string name, object[] parameters, object value)
        {
            PackageInfo.Add(name, value);

            return value;
        }

        public PackageMetaData()
        {
            PackageInfo = new Dictionary<string, object>();
        }

    }
}