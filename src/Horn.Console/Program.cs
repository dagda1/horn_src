using System;
using System.IO;
using System.Linq;
using Horn.Core;
using Horn.Core.PackageCommands;
using Horn.Core.PackageStructure;
using Horn.Core.Utils.CmdLine;
using Horn.Core.Utils.IoC;
using log4net;
using log4net.Config;

namespace Horn.Console
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log.Debug("Horn starting.........");

            XmlConfigurator.Configure();

            var output = new StringWriter();

            var parser = new SwitchParser(output, args);

            if(!parser.IsAValidRequest())
            {
                log.Error(output.ToString());
                return;
            }

            InitialiseIoC(parser.CommandArguments);

            var packageTree = IoC.Resolve<IPackageTree>().GetRootPackageTree(GetRootFolderPath());

            try
            {
                IoC.Resolve<IPackageCommand>(parser.ParsedArgs.First().Key).Execute(packageTree);
            }
            catch (UnknownInstallPackageException unpe)
            {
                log.Info(unpe.Message);
            } 
            catch(RemoteScmException scm)
            {
                log.Info(scm.Message);
            }
        }

        private static void InitialiseIoC(ICommandArgs commandArgs)
        {
            var resolver = new WindsorDependencyResolver(commandArgs);

            IoC.InitializeWith(resolver);

            log.Debug("IOC initialised.....");
        }

        private static DirectoryInfo GetRootFolderPath()
        {
            var documents = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            var rootFolder = Path.Combine(documents.Parent.FullName, PackageTree.RootPackageTreeName);

            log.DebugFormat("root folder = {0}", rootFolder);

            var ret = new DirectoryInfo(rootFolder);

            ret.Create();

            return ret;
        }
    }
}