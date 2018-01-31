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
        public async Task<AppUser> GetByUserName(string userName)
        {
            using (var sessionContainer = SessionContainer.Open())
                return await sessionContainer.Session.QueryOver<AppUser>().Where(it => it.UserName == userName).SingleOrDefaultAsync();
        }
    }
}
