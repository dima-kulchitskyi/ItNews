using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Entities;
using ItNews.Business;
using ItNews.Business.Managers;

namespace ItNews.Nhibernate.Providers
{
    public class AppUserProvider : Provider<AppUser>, IAppUserProvider
    {
        public AppUserProvider(SessionManager sessionManager) : base(sessionManager)
        {

        }
    }
}
