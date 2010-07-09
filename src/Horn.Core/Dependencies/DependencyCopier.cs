using System.Collections.Generic;
using System.IO;
using Horn.Core.Extensions;
using Horn.Core.Utils;

namespace Horn.Core.Dependencies
{
    public class DependencyCopier : WithLogging
    {
        public IEnumerable<string> CopyDependency(FileInfo file, DirectoryInfo destination)
        {
            IEnumerable<string> locationsToCopyTo = destination.Search(file.Name);

            foreach (var location in locationsToCopyTo)
            {
                CopyFile(file, location);
            }

            //Ensure we at least copy the dependency to the root folder
            CopyFile(file, Path.Combine(destination.FullName, Path.GetFileName(file.FullName)));

            return locationsToCopyTo;
        }

        private void CopyFile(FileInfo nextFile, string location)
        {
            InfoFormat("Dependency: Copying {0} to {1} ...", nextFile.FullName, location);
            nextFile.CopyTo(location, true);
        }
    }
}