using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using Horn.Core.extensions;
using Horn.Core.Utils;

namespace Horn.Core.Dependencies
{
    /// <summary>
    /// Updates project Reference element to the correct assembly info
    /// </summary>
    public class VisualStudioDependentUpdater : WithLogging, IDependentUpdater
    {
        private static readonly Regex regex = new Regex("Project\\(\".*\"\\).*\"(?<ProjectName>.*)\".*\"(?<ProjectPath>.*\\..*proj)\"",
                                              RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        private const string NamespaceName = "http://schemas.microsoft.com/developer/msbuild/2003";

        public IDictionary<string, FileInfo> ProjectFiles { get; private set; }

        public HashSet<string> SolutionFiles { get; private set; }

        public void Update(DependentUpdaterContext dependentUpdaterContext)
        {
            string firstAssemblyPath = GetFirstAssembly(dependentUpdaterContext.DependencyPaths);

            if (firstAssemblyPath == null)
                return;

            LoadSolutionFiles(dependentUpdaterContext.WorkingDirectory);
            LoadProjectFiles();
            UpdateProjectDependencies(firstAssemblyPath, dependentUpdaterContext.Dependency.Library);
        }

        protected virtual string GetVersionInfoFromAssembly(string filePath, string dependencyName)
        {
            const string template = "{0}, , processorArchitecture=MSIL";
            Assembly assembly = Assembly.LoadFrom(filePath);
            string info = assembly.GetType().AssemblyQualifiedName;
            return string.Format(template, info);
        }

        private void UpdateProjectDependencies(string dependencyAssemblyPath, string dependencyName)
        {
            string dependencyFileVersion = GetVersionInfoFromAssembly(dependencyAssemblyPath, dependencyName);

            foreach (var file in ProjectFiles)
            {
                InfoFormat("Dependency: Checking {0} for dependency on {1}", file.Key, dependencyName);

                XElement project = LoadProject(file);
                XElement dependencyElement = FindCorrectElement(project);

                if (!HasADependency(dependencyElement, dependencyName, dependencyElement)) 
                    continue;

                InfoFormat("Dependency: {0} has dependency on {1}", file.Key, dependencyName);

                UpdateReference(dependencyElement, dependencyFileVersion, dependencyName);
                SaveProject(file, project);
            }
        }

        private string GetFirstAssembly(IEnumerable<string> dependencyPaths)
        {
            return dependencyPaths.FirstOrDefault(x => x.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase));
        }

        private XElement LoadProject(KeyValuePair<string, FileInfo> file)
        {
            return XElement.Load(file.Value.FullName);
        }

        private void SaveProject(KeyValuePair<string, FileInfo> file, XElement fileContents)
        {
            fileContents.Save(file.Value.FullName);
        }

        private bool HasADependency(XElement element, string name, XElement dependencyElement)
        {
            return dependencyElement != null && GetReferenceElement(element, name) != null;
        }

        private void UpdateReference(XElement element, string versionInfo, string dependencyName)
        {
            XElement referenceElement = GetReferenceElement(element, dependencyName);
            referenceElement.Attribute("Include").Value = versionInfo;
        }

        private XElement GetReferenceElement(XElement element, string dependencyName)
        {
            return element.Elements().FirstOrDefault(el => el.Attribute("Include").Value.StartsWith(dependencyName));
        }

        private XElement FindCorrectElement(XContainer document)
        {
            return document.Elements(XName.Get("ItemGroup", NamespaceName)).FirstOrDefault(el => el.Elements(XName.Get("Reference", NamespaceName)).Count() > 0);
        }

        private void LoadProjectFiles()
        {
            InfoFormat("Dependency: Loading project files ...");

            if (ProjectFiles.Count > 0)
                return;

            SolutionFiles.ForEach(FetchProjectFilesFromSolutionFile);
        }

        private void FetchProjectFilesFromSolutionFile(string solutionPath)
        {
            InfoFormat("Dependency: Loading project files for {0} ...", Path.GetFileName(solutionPath));

            string solutionContents = ReadFileContents(solutionPath);

            MatchCollection matchCollection = regex.Matches(solutionContents);

            InfoFormat("Dependency: Found a possible {0} projects", matchCollection.Count);

            foreach (Match match in matchCollection)
            {
                if ( !match.Success)
                    continue;

                AddProject(match, Path.GetDirectoryName(solutionPath));
            }
        }

        private void AddProject(Match match, string workingDirectory)
        {
            var projectName = match.Groups["ProjectName"].Value;
            var projectPath = match.Groups["ProjectPath"].Value;

            projectPath = Path.Combine(workingDirectory, projectPath);

            if ( ShouldAddProject(projectName, projectPath) )
            {
                InfoFormat("Dependency: Adding project files {0} @ {1}...", projectName, projectPath);
                ProjectFiles.Add(projectName, new FileInfo(projectPath));
            }
        }

        private bool ShouldAddProject(string projectName, string projectPath)
        {
            return !ProjectAlreadyLoaded(projectName) && !IsAWebProject(projectPath) && ProjectsExists(projectPath);
        }

        private bool ProjectsExists(string projectPath)
        {
            return File.Exists(projectPath);
        }

        private bool IsAWebProject(string projectPath)
        {
            return projectPath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase);
        }

        private bool ProjectAlreadyLoaded(string projectName)
        {
            return ProjectFiles.ContainsKey(projectName);
        }

        private string ReadFileContents(string path)
        {
            string fileContents;

            using (var fileReader = new StreamReader(path))
            {
                fileContents = fileReader.ReadToEnd();
            }

            return fileContents;
        }

        private void LoadSolutionFiles(DirectoryInfo workingDirectory)
        {
            if (SolutionFiles.Count() > 0)
                return;

            Info("Dependency: Loading solution files ...");

            IEnumerable<string> collection = workingDirectory.Search("*.sln");
            SolutionFiles = new HashSet<string>(collection);

            InfoFormat("Dependency: Found {0} solution file(s)", SolutionFiles.Count());
        }

        public VisualStudioDependentUpdater()
        {
            ProjectFiles = new Dictionary<string, FileInfo>();
            SolutionFiles = new HashSet<string>();
        }
    }
}