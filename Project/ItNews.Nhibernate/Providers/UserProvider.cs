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
    public class UserProvider : Provider<AppUser>, IUserProvider
    {
        public UserProvider(SessionManager sessionManager) : base(sessionManager)
        {

        }

        public Task<AppUser> GetByUserName(string userName)
        {
            return sessionManager.GetExistingOrOpenSession().QueryOver<AppUser>().Where(it => it.UserName == userName).SingleOrDefaultAsync();
        }
    }
}
