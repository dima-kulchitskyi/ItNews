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
        public Task<IList<AppUser>> GetAllUsers()
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<AppUser>().OrderBy(m => m.UserName).Asc.ListAsync());
        }

        public Task<AppUser> GetByUserName(string userName)
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<AppUser>().Where(it => it.UserName == userName).SingleOrDefaultAsync());
        }
    }
}
