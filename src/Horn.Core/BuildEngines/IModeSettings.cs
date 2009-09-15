using System.Collections.Generic;

namespace Horn.Core.BuildEngines
{
    public interface IModeSettings
    {
        string Name { get; }
        IList<string> Tasks { get; }
        IDictionary<string, string> Parameters { get; }
        void AssignTasks( string[] tasks );
        void AssignParameters( string[] parameters );
    }
}
