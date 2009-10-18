using System.IO;
using horn.services.core.Value;

namespace Horn.Services.Core.Builder
{
    public interface ISiteStructureBuilder
    {
        void Build();

        void Initialise();

        void Run();

        bool ServiceStarted { get; set; }
    }
}
