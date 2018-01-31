using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ItNews
{
    public class ControllerBuilderConfig
    {
        public static void RegisterNamespaces(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.DefaultNamespaces.Add(WebConfigurationManager.AppSettings["DefaultRoutesNamespace"]);
        }
    }
}