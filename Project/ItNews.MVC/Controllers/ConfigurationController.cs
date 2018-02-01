using ItNews.Business;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Mvc.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpPost]
        public ActionResult ProviderType(string providerType)
        {
            if (string.IsNullOrEmpty(providerType))
                return HttpNotFound();

            var config = DependencyResolver.Current.GetService<ApplicationVariables>();

            if (config.DataSourceProviderType != providerType)
            {
                config.DataSourceProviderType = providerType;
                DependencyResolver.Current.GetService<IAuthenticationManager>().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }

            return RedirectToAction("Index", "Article");
        }
    }
}
