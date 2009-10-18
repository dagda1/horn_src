using System;
using Horn.Core.BuildEngines;
using log4net;

namespace Horn.Spec.Framework.Stubs
{
    public class StubProcess : IProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (StubProcess));

        public string GetLineOrOutput()
        {
            return null;
        }

        public void WaitForExit()
        {
            Console.WriteLine("WaitForExit called in the StubProcess");
        }
    }
}