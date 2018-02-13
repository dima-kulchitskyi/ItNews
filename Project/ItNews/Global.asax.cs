using ItNews.Business;
using ItNews.Mvc.ModelBinders.Article;
using ItNews.Web;
using ItNews.Web.DependencyInjection;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ItNews
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(int), new PageNumberModelBinder());

            ControllerBuilderConfig.RegisterNamespaces(ControllerBuilder.Current);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            UnityConfiguration.Initialize();
        }
    }
}
