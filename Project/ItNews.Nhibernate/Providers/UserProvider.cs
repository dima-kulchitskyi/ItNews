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
        public async  Task<IList<AppUser>> GetAllUsers()
        {
            using (var sessionContainer = SessionContainer.Open())
                return await sessionContainer.Session.QueryOver<AppUser>().OrderBy(m => m.UserName).Asc.ListAsync();
        }

        public async Task<AppUser> GetByUserNameAsync(string userName)
        {
            using (var sessionContainer = SessionContainer.Open())
                return await sessionContainer.Session.QueryOver<AppUser>().Where(it => it.UserName == userName).SingleOrDefaultAsync();
        }
    }
}
