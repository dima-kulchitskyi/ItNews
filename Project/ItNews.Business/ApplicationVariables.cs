using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business
{
    public static class ApplicationVariables
    {
        static ApplicationVariables()
        {
            DataSourceProviderType = ConfigurationManager.AppSettings["DefaultProviderType"];
        }

        public static string DataSourceProviderType { get; set; }
    }
}
