using System;

namespace Horn.Core.Dependencies
{
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException(string cause)
            : base(String.Format("{0} is a dependent of itself", cause))
        {
        }
    }
}