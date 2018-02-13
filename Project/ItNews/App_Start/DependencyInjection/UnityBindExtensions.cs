using ItNews.DependencyInjection.LifetimeManagers;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace ItNews.Web.DependencyInjection
{
    public static class UnityBindExtentions
    {
        public static IUnityContainer RegisterTypeInRequestScope<TInterface, TImplementation>(this IUnityContainer container, string name = null) where TImplementation : TInterface
        {
            if(name == null)
                return container.RegisterType<TInterface, TImplementation>(new PerRequestLifetimeManager());

            return container.RegisterType<TInterface, TImplementation>(name, new PerRequestLifetimeManager());
        }

        public static IUnityContainer RegisterTypeInSingletonScope<TInterface, TImplementation>(this IUnityContainer container, string name = null) where TImplementation : TInterface
        {
            return container.RegisterType<TInterface, TImplementation>(name, new ContainerControlledLifetimeManager());
        }
    }
}