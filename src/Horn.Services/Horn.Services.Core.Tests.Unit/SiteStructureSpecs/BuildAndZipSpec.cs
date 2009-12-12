using System;
using System.Collections.Generic;
using System.IO;
using Horn.Core.Dsl;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Tree.MetaDataSynchroniser;
using Horn.Core.Utils;
using Horn.Core.Utils.CmdLine;
using Horn.Core.Utils.IO;
using Horn.Core.Utils.IoC;
using Horn.Services.Core.Builder;
using Horn.Services.Core.Tests.Unit.Doubles;
using horn.services.core.Value;
using Horn.Spec.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Horn.Services.Core.Tests.Unit.SiteStructureSpecs
{
    //public class Copy : ContextSpecification
    //{
    //    private DirectoryInfo sourceTest;
    //    private DirectoryInfo destinationTest;

    //    protected override void establish_context()
    //    {
    //        sourceTest = new DirectoryInfo(@"C:\temp\Horn\horn\.horn");

    //        destinationTest = new DirectoryInfo(@"C:\zip");
    //    }

    //    protected override void because()
    //    {
    //    }

    //    public void SynchDirectories(DirectoryInfo source, DirectoryInfo destination)
    //    {
    //        foreach (var directory in source.GetDirectories())
    //        {
    //            var destinationDirectory = new DirectoryInfo(Path.Combine(destination.FullName, directory.Name));

    //            if(!destinationDirectory.Exists)
    //                destinationDirectory.Create();

    //            SynchDirectories(directory, destinationDirectory);
    //        }
    //    }

    //    [Test]
    //    public void CopyDirectoriesTest()
    //    {
    //        SynchDirectories(sourceTest, destinationTest);
    //    }
    //}

    public class When_a_package_is_being_built : ContextSpecification
    {
        protected IFileSystemProvider fileSystemProvider;
        protected IPackageCommand packageBuilder;
        protected ICommandArgs commandArgs;
        protected IPackageTree rootPackageTree;
        protected Package package;
        protected IBuildMetaData buildMetaData;
        protected ISiteStructureBuilder siteStructureBuilder;

        public override void before_each_spec()
        {
            var dependencyResolver = MockRepository.GenerateStub<IDependencyResolver>();
            fileSystemProvider = MockRepository.GenerateStub<IFileSystemProvider>();
            packageBuilder = MockRepository.GenerateStub<IPackageCommand>();
            rootPackageTree = MockRepository.GenerateStub<IPackageTree>();
            buildMetaData = MockRepository.GenerateStub<IBuildMetaData>();

            commandArgs = new CommandArgs("horn", false, null, false, null);

            dependencyResolver.Stub(x => x.HasComponent<ICommandArgs>()).Return(true);

            dependencyResolver.Stub(x => x.Resolve<ICommandArgs>()).Return(commandArgs);

            dependencyResolver.Stub(x => x.Resolve<IPackageCommand>("install")
                ).Return(packageBuilder);

            IoC.InitializeWith(dependencyResolver);

            rootPackageTree.Stub(x => x.Result).Return(new DirectoryInfo(@"z:\horn"));

            buildMetaData.ProjectInfo= new Dictionary<string, object>();

            rootPackageTree.Stub(x => x.GetAllPackageMetaData()).Return(new List<IBuildMetaData> {buildMetaData});

            rootPackageTree.Stub(x => x.Name).Return("horn");

            var category = new Category(null, rootPackageTree);

            fileSystemProvider.Stub(x => x.GetFiles(Arg<DirectoryInfo>.Is.TypeOf, Arg<string>.Is.TypeOf)).Return(
                new List<FileInfo>
                    {
                        new FileInfo(string.Format("horn-{0}.zip",
                                                   new DateTime(2009, 10, 30).ToString(FileSystemProvider.FileDateFormat))),
                        new FileInfo(string.Format("horn-{0}.zip",
                                                   new DateTime(2009, 10, 29).ToString(FileSystemProvider.FileDateFormat))),
                        new FileInfo(string.Format("horn-{0}.zip",
                                                   new DateTime(2009, 10, 31).ToString(FileSystemProvider.FileDateFormat)))
                    }.ToArray());

            package = new PackageDouble(category, buildMetaData);

            string zipFileName = string.Format("{0}-{1}.zip", package.Name, DateTime.Now.ToString(FileSystemProvider.FileDateFormat));

            fileSystemProvider.Stub(
                x => x.ZipFolder(Arg<DirectoryInfo>.Is.TypeOf, Arg<DirectoryInfo>.Is.TypeOf, Arg<string>.Is.TypeOf)).
                Return(new FileInfo(zipFileName));
        }

        protected override void establish_context()
        {           
            var metaDataSynchroniser = MockRepository.GenerateStub<IMetaDataSynchroniser>();

            siteStructureBuilder = new SiteStructureBuilder(metaDataSynchroniser, fileSystemProvider, @"z:\dropthat\");
        }

        protected override void because()
        {
            var newDirectory = new DirectoryInfo(@"z:\hornthing\");

            var tempDirectory = new DirectoryInfo(@"z:\temp");
            
            siteStructureBuilder.BuildAndZipPackage(rootPackageTree, fileSystemProvider, package, newDirectory, tempDirectory);
        }

        [Test]
        public void Then_a_unique_zip_file_is_created()
        {
            fileSystemProvider.AssertWasCalled(x => x.ZipFolder(Arg<DirectoryInfo>.Is.TypeOf, Arg<DirectoryInfo>.Is.TypeOf, Arg<string>.Is.TypeOf));
        }
    }   
}