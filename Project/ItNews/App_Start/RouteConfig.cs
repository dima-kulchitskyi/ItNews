using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace ItNews.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static void SetRoutesDefaultNamespace(RouteCollection routes)
        {
            var defaultNamespace = WebConfigurationManager.AppSettings["DefaultRoutesNamespace"];
            foreach (Route r in routes)
                if (r.DataTokens != null && !r.DataTokens.ContainsKey("Namespaces"))
                    r.DataTokens.Add("Namespaces", new string[] { defaultNamespace });
        }
    }
}
