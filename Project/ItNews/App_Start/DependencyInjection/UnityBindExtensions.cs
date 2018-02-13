using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace ItNews.Web.DependencyInjection
{
    public static class UnityBindExtentions
    {
        public static IUnityContainer RegisterTypeInRequestScope<TInterface, TImplementation>(this IUnityContainer container, string name = null) where TImplementation : TInterface
        {
            return container.RegisterType<TInterface, TImplementation>(name, new PerRequestLifetimeManager());
        }

        public static IUnityContainer RegisterTypeInSingletonScope<TInterface, TImplementation>(this IUnityContainer container, string name = null) where TImplementation : TInterface
        {
            return container.RegisterType<TInterface, TImplementation>(name, new ContainerControlledLifetimeManager());
        }
    }
}