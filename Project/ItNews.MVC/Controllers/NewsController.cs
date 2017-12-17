using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItNews.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}