using System.IO;
using Castle.MicroKernel.Registration;
using Horn.Core.Utils.IoC;
using Horn.Services.Core.Builder;
using Parameter = Castle.MicroKernel.Registration.Parameter;

namespace Horn.Services.Core.IoCServices
{
    public class ServicesDependencyResolver : WindsorDependencyResolver
    {
        public ServicesDependencyResolver(DirectoryInfo dropDirectory) : base(null)
        {
            innerContainer.Register(
                Component.For<ISiteStructureBuilder>()
                            .ImplementedBy<SiteStructureBuilder>()
                            .Parameters(Parameter.ForKey("dropDirectoryPath").Eq(dropDirectory.FullName))
                            .LifeStyle.Transient
                );
        }
    }
}