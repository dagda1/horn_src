using System;

namespace horn.services.core.Value
{
    public interface IResource
    {
        bool IsRoot { get; }

        string Name { get; }

        IResource Parent { get; }

        string Url { get; set; }
    }
}