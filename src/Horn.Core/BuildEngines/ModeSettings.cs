using System.Collections.Generic;
using Horn.Core.Extensions;

namespace Horn.Core.BuildEngines
{
    public class ModeSettings : IModeSettings
    {
        public ModeSettings( string name )
        {
            Name = name;
            Tasks = new List<string>();
            Parameters = new Dictionary<string, string>();
        }

        public string Name { get; private set; }
        public IList<string> Tasks { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }

        public void AssignTasks( string[] tasks )
        {
            tasks.ForEach( task => Tasks.Add( task ) );
        }

        public void AssignParameters( string[] parameters )
        {
            if ((parameters == null) || (parameters.Length == 0))
                return;

            parameters.ForEach(x =>
            {
                var parts = x.Split('=');
                Parameters.Add(parts[0], parts[1]);
            });

        }
    }
}
