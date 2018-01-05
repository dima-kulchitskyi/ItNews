using ItNews.Web;
using ItNews.Web.DependencyInjection;
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

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.SetRoutesDefaultNamespace(RouteTable.Routes);

            UnityConfiguration.Initialize();
        }
    }
}
