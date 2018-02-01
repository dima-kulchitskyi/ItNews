using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business
{
    public class ServerVariables
    {
        public ServerVariables()
        {
            DataSourceProviderType = ConfigurationManager.AppSettings["DefaultProviderType"];
        }

        public string DataSourceProviderType { get; set; }
    }
}
