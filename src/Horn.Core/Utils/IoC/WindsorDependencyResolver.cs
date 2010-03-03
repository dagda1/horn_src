using System;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Horn.Core.BuildEngines;
using Horn.Core.Config;
using Horn.Core.Dsl;
using Horn.Core.exceptions;
using Horn.Core.GetOperations;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.SCM;
using Horn.Core.Tree.MetaDataSynchroniser;
using System.Reflection;
using Horn.Core.Dependencies;
using Horn.Core.Utils.CmdLine;
using Parameter = Castle.MicroKernel.Registration.Parameter;

namespace Horn.Core.Utils.IoC
{
	public class WindsorDependencyResolver : IDependencyResolver
	{
		protected readonly WindsorContainer innerContainer;

		public void AddComponentInstance<Ttype>(string key, Type service, Ttype instance)
		{
			innerContainer.Kernel.AddComponentInstance(key, service, instance);
		}

		public bool HasComponent<TService>()
		{
			return innerContainer.Kernel.HasComponent(typeof(TService));
		}

		//TODO: Remove
		public IWindsorContainer GetContainer()
		{
			return innerContainer;
		}

		public T Resolve<T>()
		{
			return innerContainer.Resolve<T>();
		}

		public T Resolve<T>(string key)
		{
			return innerContainer.Resolve<T>(key);
		}

		public WindsorDependencyResolver(ICommandArgs commandArgs)
		{
			innerContainer = new WindsorContainer();

			if(commandArgs != null)
			    innerContainer.Kernel.AddComponentInstance<ICommandArgs>(typeof(ICommandArgs), commandArgs);

			innerContainer.Kernel.Resolver.AddSubResolver(new EnumerableResolver(innerContainer.Kernel));

			innerContainer.Register(
				Component.For<IBuildConfigReader>()
							.Named("boo")
							.ImplementedBy<BooBuildConfigReader>()
							.LifeStyle.Transient
							);

			innerContainer.Register(
				Component.For<SVNSourceControl>()
							.Named("Svn")
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<MercurialSourceControl>()
							.Named("Hg")
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IPackageCommand>()
							.Named("install")
							.ImplementedBy<PackageBuilder>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IGet>()
							.Named("get")
							.ImplementedBy<Get>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IFileSystemProvider>()
							.Named("filesystemprovider")
							.ImplementedBy<FileSystemProvider>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IProcessFactory>()
							.Named("processfactory")
							.ImplementedBy<DiagnosticsProcessFactory>()
							.LifeStyle.Transient

				);

			innerContainer.Register(
				Component.For<IMetaDataSynchroniser>()
							.ImplementedBy<MetaDataSynchroniser>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IPackageTree>()
							.ImplementedBy<PackageTree>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IEnvironmentVariable>()
							.ImplementedBy<EnvironmentVariable>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IShellRunner>()
							.ImplementedBy<CommandLineRunner>()
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<IDependencyDispatcher>()
					.ImplementedBy<DependencyDispatcher>()
					.LifeStyle.Transient,

				Component.For<IDependentUpdaterExecutor>()
					.ImplementedBy<DependentUpdaterExecutor>()
					.LifeStyle.Transient,

				AllTypes.Of<IDependentUpdater>().FromAssembly(Assembly.GetExecutingAssembly())
					.WithService.FirstInterface().Configure(config => config.LifeStyle.Transient)
				);

			var gitBinDirectory = new GitBinDirectoryFinder().FindPreferred();

			innerContainer.Register(
				Component.For<SourceControl>()
							.ImplementedBy<GitSourceControl>()
							.Parameters(
								Parameter.ForKey("url").Eq(MetaDataSynchroniser.PackageTreeUri),
								Parameter.ForKey("gitBinDirectory").Eq(gitBinDirectory),
								Parameter.ForKey("BranchName").Eq(HornConfig.Settings.PackageTreeBranch))
							.LifeStyle.Transient
				);

			innerContainer.Register(
				Component.For<GitSourceControl>()
							.Named("Git")
							.Parameters(Parameter.ForKey("gitBinDirectory").Eq(gitBinDirectory))
							.LifeStyle.Transient
				);

			if (!string.IsNullOrEmpty(HornConfig.Settings.BashDirectory))
			{
				innerContainer.Register(
					Component.For<IGitCommand>()
						.ImplementedBy<BashInvokedGitCommand>());
			}
		}
	}
}