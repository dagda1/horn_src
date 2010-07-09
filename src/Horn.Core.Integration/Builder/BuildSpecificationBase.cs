using System;
using System.IO;
using Horn.Core.BuildEngines;
using Horn.Core.Extensions;
using Horn.Core.PackageStructure;
using Horn.Framework.helpers;
using Horn.Spec.Framework.Extensions;
using Rhino.Mocks;

namespace Horn.Core.Integration.Builder
{
    public abstract class BuildSpecificationBase : Specification
    {
        protected string workingPath;
        protected string outputPath;
        protected BuildEngine buildEngine;
        protected IPackageTree packageTree;

        public static bool IsRunningFromCIBuild
        {
            get
            {
                return (DirectoryHelper.GetBaseDirectory().IndexOf("net-3.5") > -1);   
            }
        }

        public static string ResolveRootPath(string executionBase)
        {
            if (!IsRunningFromCIBuild)
                return new DirectoryInfo(executionBase.ResolvePath()).Parent.FullName;

            if(executionBase.IndexOf("debug") > -1)
                return new DirectoryInfo(executionBase).Parent.Parent.Parent.FullName;

            return new DirectoryInfo(executionBase).Parent.Parent.FullName;
        }

        protected string GetRootPath()
        {
            outputPath = CreateDirectory("Output");

            workingPath = CreateDirectory("Working");

            packageTree = MockRepository.GenerateStub<IPackageTree>();

            packageTree.Stub(x => x.OutputDirectory).Return(new DirectoryInfo(outputPath));

            packageTree.Stub(x => x.Result).Return(new DirectoryInfo(outputPath));

            var executionBase = AppDomain.CurrentDomain.BaseDirectory;

            return ResolveRootPath(executionBase);
        }

        protected string CreateDirectory(string directoryName)
        {
            var path = Path.Combine(DirectoryHelper.GetBaseDirectory(), directoryName);

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);

            return path;
        }
    }
}