using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Lifetime;

namespace ItNews.Web.DependencyInjection
{
    public static class UnityBindExtentions
    {
        public static IUnityContainer RegisterTypeInRequestScope<TInterface, TImplementation>(this IUnityContainer container) where TImplementation : TInterface
        {
            return container.RegisterType<TInterface, TImplementation>(new HierarchicalLifetimeManager());
        }

        public static IUnityContainer RegisterTypeInSingletonScope<TInterface, TImplementation>(this IUnityContainer container) where TImplementation : TInterface
        {
            return container.RegisterType<TInterface, TImplementation>(new ContainerControlledLifetimeManager());
        }
    }
}