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
        public static IUnityContainer RegisterTypeInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
            return container;
        }

        public static IUnityContainer RegisterTypeInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}