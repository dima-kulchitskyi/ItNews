using System;
using System.Linq;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;



namespace ItNews.Web.DependencyInjection
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityConfiguration
    {

        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              UnityRegistrations.RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Initialize()
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(Container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

    }
}