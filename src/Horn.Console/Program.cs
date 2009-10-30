using System;
using System.IO;
using System.Linq;
using Horn.Console.Config;
using Horn.Core;
using Horn.Core.exceptions;
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

            var packageTree = IoC.Resolve<IPackageTree>().GetRootPackageTree(GetRootFolderPath(parser.CommandArguments));

            try
            {
                IoC.Resolve<IPackageCommand>(parser.ParsedArgs.First().Key).Execute(packageTree);
            }
            catch (UnknownInstallPackageException unpe)
            {
                log.Info(unpe.Message);
            }
            catch (BuildFailedException bfe)
            {
                log.Info(bfe.Message);
            }
            catch(RemoteScmException scm)
            {
                log.Info(scm.Message);
            }
            catch(EnvironmentVariableNotFoundException eve)
            {
                log.Info(eve.Message);
            }
        }

        private static void InitialiseIoC(ICommandArgs commandArgs)
        {
            var resolver = new WindsorDependencyResolver(commandArgs);

            IoC.InitializeWith(resolver);

            log.Debug("IOC initialised.....");
        }

        private static DirectoryInfo GetRootFolderPath(ICommandArgs commandArgs)
        {
            string rootFolder;

            if (!String.IsNullOrEmpty(commandArgs.OutputPath))
            {
                rootFolder = Path.Combine(commandArgs.OutputPath, PackageTree.RootPackageTreeName);
            }
            else
            {
                try
                {
                    var rootDir = new DirectoryInfo(HornConfig.Settings.HornRootDirectory);

                    if (!rootDir.Exists)
                        rootDir.Create();

                    rootFolder = rootDir.FullName;
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                    var documents = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

                    rootFolder = Path.Combine(documents.Parent.FullName, PackageTree.RootPackageTreeName);   
                }
            }            

            log.DebugFormat("root folder = {0}", rootFolder);

            var ret = new DirectoryInfo(rootFolder);

            ret.Create();

            return ret;
        }
    }
}