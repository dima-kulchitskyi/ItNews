using ItNews.Business;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!string.IsNullOrEmpty(providerType))
                DependencyResolver.Current.GetService<ServerVariables>().DataSourceProviderType = providerType;

            return RedirectToAction("Index", "Article");
        }
    }
}
