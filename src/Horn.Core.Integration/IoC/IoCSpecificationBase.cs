using Horn.Core.Utils.IoC;
using Horn.Spec.Framework.doubles;

namespace Horn.Core.Integration.IoC
{
    public class IoCSpecificationBase : Specification
    {
        protected override void Before_each_spec()
        {
            var resolver = new WindsorDependencyResolver(new CommandArgsDouble("horn"));

            global::IoC.InitializeWith(resolver);            
        }

        protected override void Because()
        {
        }       
    }
}