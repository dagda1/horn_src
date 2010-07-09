using System;
using System.IO;
using Horn.Core.Extensions;
using Horn.Core.GetOperations;
using Horn.Core.PackageStructure;

namespace Horn.Core.Dsl
{
    public class RepositoryElement : IRepositoryElement
    {
        private IPackageTree repositoryTree;
        private IPackageTree packageTreeToExportTo;
        private const string PackageTreeNullErrorMessage =
            "You must call PrepareRepository before export in the RepositoryElement class.  The {0} member is null.";

        public string ExportPath { get; private set; }

        public string IncludePath { get; private set; }

        public string RepositoryName { get; private set; }

        public virtual void Export()
        {
            if (repositoryTree == null)
                throw new AccessViolationException(string.Format(PackageTreeNullErrorMessage, "repositoryTree"));

            if (packageTreeToExportTo == null)
                throw new AccessViolationException(string.Format(PackageTreeNullErrorMessage, "packageTreeToExportTo"));

            var source = repositoryTree.WorkingDirectory.GetFileSystemObjectFromParts(IncludePath);

            var destination = packageTreeToExportTo.WorkingDirectory.GetFileSystemObjectFromParts(ExportPath, source.IsFile());

            CopyElement(source, destination);
        }

        public virtual IRepositoryElement PrepareRepository(IPackageTree packageToExportTo, IGet get)
        {
            packageTreeToExportTo = packageToExportTo;

            var root = packageToExportTo.Root;
            var buildMetaData = root.GetBuildMetaData(RepositoryName);

            repositoryTree = root.RetrievePackage(RepositoryName);

            get.From(buildMetaData.SourceControl).ExportTo(repositoryTree);            

            return this;
        }

        protected virtual void CopyElement(FileSystemInfo source, FileSystemInfo destination)
        {
            if (source.FullName.PathIsFile())
            {
                File.Copy(source.FullName, destination.FullName, true);
                return;
            }

            var directoryInfo = (DirectoryInfo)source;
            var destinationDirectory = (DirectoryInfo)destination;
            directoryInfo.CopyToDirectory(destinationDirectory, true);
        }

        public RepositoryElement(string repositoryName, string includePath, string exportPath)
        {
            RepositoryName = repositoryName;
            IncludePath = includePath;
            ExportPath = exportPath;
        }
    }
}