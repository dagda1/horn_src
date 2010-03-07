using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Dependencies;
using Horn.Spec.Framework.Stubs;

namespace Horn.Core.Spec.Dependencies
{
    public abstract class visual_studio_dependent_updater_context : Specification
    {
        protected VisualStudioDependentUpdater dependentUpdater;
        protected IEnumerable<string> dependencyFilePaths;
        protected string workingPath;
        protected string dependencyFilename = "dummydependency.dll";
        protected string projectPath;
        public DependentUpdaterContext updaterContext;
        protected string solutionContents = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Fake.Project"", ""Fake.Project\Fake.Project.csproj"", ""{093CFA20-D720-4C58-9B8F-754F03A40D16}""";
        protected string projectContents = @"<?xml Version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""3.5"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>9.0.30729</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{093CFA20-D720-4C58-9B8F-754F03A40D16}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Horn.Core</RootNamespace>
    <AssemblyName>Horn.Core</AssemblyName>
    <TargetFrameworkVersion>v3.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|x86' "">
    <DebugSymbols>true</DebugSymbols>
    <OutputPath>bin\x86\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <DebugType>full</DebugType>
    <PlatformTarget>x86</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|x86' "">
    <OutputPath>bin\x86\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <Optimize>true</Optimize>
    <DebugType>pdbonly</DebugType>
    <PlatformTarget>x86</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""dummydependency.dll, Version=2.0.0.0, Culture=neutral, PublicKeyToken=32c39770e9a21a67, processorArchitecture=MSIL"">
      <SpecificVersion>False</SpecificVersion>
      <HintPath>..\..\lib\dummydependency.dll</HintPath>
    </Reference>
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""..\CommonAssemblyInfo.cs"">
      <Link>CommonAssemblyInfo.cs</Link>
    </Compile>
    <Compile Include=""BuildEngines\Dependency.cs"" />
  </ItemGroup>
  <ItemGroup>
    <Content Include=""BuildConfigs\horn.boo"" />
    <Content Include=""BuildConfigs\log4net.boo"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""Horn.Core.build"" />
    <None Include=""Model\BuildEngine.cd"" />
    <None Include=""Model\SemanticModel.cd"" />
    <None Include=""Model\SourceControl.cd"" />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>";
        protected PackageTreeStub _packageTreeStub;

        protected override void Before_each_spec()
        {
            workingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString());
            dependencyFilePaths = new[] {Path.Combine(workingPath, dependencyFilename) };

            _packageTreeStub = CreateStub<PackageTreeStub>(new[] {workingPath});

            CreateDirectories();
            CreateDummySolutionFile();
            CreateDummyProjectFile();

            dependentUpdater = new VisualStudioDependentUpdaterDouble();

            updaterContext = new DependentUpdaterContext(_packageTreeStub, dependencyFilePaths, new Dependency("", Path.GetFileNameWithoutExtension(dependencyFilename)));
        }

        protected override void Because() { }

        private void CreateDummyProjectFile()
        {
            string projectDirectory = Path.Combine(_packageTreeStub.WorkingDirectory.FullName, "Fake.Project");
            projectPath = Path.Combine(projectDirectory, "Fake.Project.csproj");
            var info = new DirectoryInfo(projectDirectory);

            if ( !info.Exists)
                info.Create();

            File.WriteAllText(projectPath, projectContents);
        }

        private void CreateDirectories()
        {
            Directory.CreateDirectory(workingPath);
            _packageTreeStub.WorkingDirectory.Create();
        }

        protected override void After_each_spec()
        {
            if (Directory.Exists(workingPath))
                Directory.Delete(workingPath, true);
        }

        private void CreateDummySolutionFile()
        {
            string solutionPath = Path.Combine(_packageTreeStub.WorkingDirectory.FullName, "dummy.sln");
            File.WriteAllText(solutionPath, solutionContents);
        }
    }
}