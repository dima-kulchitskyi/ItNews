using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryProvider
{
    public class UserProvider : MemoryProvider<AppUser>, IUserProvider
    {
        public async Task<AppUser> GetByUserNameAsync(string userName)
        {
            return (await base.GetList()).Where(it => it.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
