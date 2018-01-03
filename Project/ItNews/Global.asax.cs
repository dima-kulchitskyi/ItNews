using ItNews.Mvc;
using ItNews.Mvc.DependencyInjection;
using ItNews.Web;
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

            NinjectInitialization.Initialize(new NinjectRegistrations());
        }
    }
}
