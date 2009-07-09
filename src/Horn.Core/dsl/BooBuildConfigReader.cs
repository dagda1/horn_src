using System;
using System.IO;
using Horn.Core.PackageStructure;
using Rhino.DSL;

namespace Horn.Core.Dsl
{
    public class BooBuildConfigReader : IBuildConfigReader
    {

        private BooConfigReader configReader;
        protected DslFactory factory;

        public IPackageTree PackageTree { get; private set; }

        public IBuildMetaData GetBuildMetaData(string packageName)
        {
            if (factory == null)
                throw new ArgumentNullException("You have not called SetDslFactory on class BooBuildConfigReader");

            return CreateBuildMetaData(PackageTree.CurrentDirectory, packageName);
        }

        public IBuildMetaData GetBuildMetaData(IPackageTree packageTree, string buildFile)
        {
            if (factory == null)
                throw new ArgumentNullException("You have not called SetDslFactory on class BooBuildConfigReader");

            return CreateBuildMetaData(packageTree.CurrentDirectory, packageTree.FullName);
        }

        public virtual IBuildConfigReader SetDslFactory(IPackageTree packageTree)
        {
            PackageTree = packageTree;

            factory = new DslFactory
                            {
                                BaseDirectory = packageTree.CurrentDirectory.FullName
                            };

            factory.Register<BooConfigReader>(new ConfigReaderEngine());

            return this;
        }



        private IBuildMetaData CreateBuildMetaData(DirectoryInfo buildFolder, string buildFile)
        {
            var buildFileResolver = new BuildFileResolver();
            var buildFilePath = buildFileResolver.Resolve(buildFolder, buildFile).BuildFile;

            try
            {
                configReader = factory.Create<BooConfigReader>(buildFilePath);
            }
            catch (InvalidOperationException e)
            {
                throw new MissingBuildFileException(buildFolder, e);
            }

            configReader.Prepare();

            return configReader.BuildMetaData;
        }



    }
}