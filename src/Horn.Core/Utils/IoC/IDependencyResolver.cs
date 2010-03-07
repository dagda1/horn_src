using System;
using Castle.Windsor;

namespace Horn.Core.Utils.IoC
{
    public interface IDependencyResolver
    {
        void AddComponentInstance<Ttype>(string key, Type service, Ttype instance);

        bool HasComponent<TService>();

        //TODO: Remove
        IWindsorContainer GetContainer();

        T Resolve<T>();

        T Resolve<T>(string key);
    }
}