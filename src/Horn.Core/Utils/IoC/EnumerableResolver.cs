using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel;
namespace Horn.Core.Utils.IoC
{
    public class EnumerableResolver : ISubDependencyResolver
    {

        private readonly IKernel kernel;


        public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver,
                               ComponentModel model,
                               DependencyModel dependency)
        {
            bool result = dependency.TargetType != null &&
                          dependency.TargetType.GetGenericArguments().Length != 0 &&
                          typeof(IEnumerable<>)
                              .MakeGenericType(dependency.TargetType.GetGenericArguments()[0])
                              .IsAssignableFrom(dependency.TargetType);
            return result;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver parentResolver,
                              ComponentModel model,
                              DependencyModel dependency)
        {
            Type t = dependency.TargetType.GetGenericArguments()[0];
            return kernel.ResolveAll(t, null);
        }



        public EnumerableResolver(IKernel kernel)
        {
            this.kernel = kernel;

        }



    }
}