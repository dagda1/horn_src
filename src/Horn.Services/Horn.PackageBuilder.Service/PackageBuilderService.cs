using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Horn.Services.Core.Config;
using Horn.Services.Core.Builder;
using Horn.Services.Core.IoCServices;
using log4net;
using log4net.Config;

namespace Horn.PackageBuilder.Service
{
    public partial class PackageBuilderService : ServiceBase
    {
        private ISiteStructureBuilder siteStructureBuilder;

        private Thread builderThread;

        private static readonly ILog log = LogManager.GetLogger(typeof (PackageBuilderService));

        public PackageBuilderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("We have began.");

            Initialise();

            builderThread = new Thread(siteStructureBuilder.Run);

            siteStructureBuilder.ServiceStarted = true;

            builderThread.Start();

            log.Info("Builder thread started.");
        }

        private void Initialise()
        {
            try
            {
                XmlConfigurator.Configure();

                var dropDirectory = new DirectoryInfo(HornConfig.Settings.DropDirectory);

                var resolver = new ServicesDependencyResolver(dropDirectory);

                IoC.InitializeWith(resolver);

                siteStructureBuilder = IoC.Resolve<ISiteStructureBuilder>();
            }
            catch (Exception ex)
            {
                Debugger.Break();

                log.Error(ex);

                throw;
            }

            log.Debug("IOC initialised.....");
        }

        protected override void OnStop()
        {
            if(!builderThread.IsAlive)
                return;

            siteStructureBuilder.ServiceStarted = false;

            builderThread.Interrupt();

            builderThread.Abort();
        }
    }
}
