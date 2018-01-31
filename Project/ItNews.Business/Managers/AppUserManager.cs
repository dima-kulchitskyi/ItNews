using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class AppUserManager : Manager<AppUser, IUserProvider>
    {
        private IUserProvider UserProvider;

        public AppUserManager(IUserProvider provider) : base(provider)
        {
            UserProvider = provider;
        }
    }
}
