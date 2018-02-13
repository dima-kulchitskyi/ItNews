using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.FileProvider
{
    public class UserProvider : FileProvider<AppUser>, IUserProvider
    {
        public async Task<AppUser> GetByUserName(string userName)
        {
            return (await base.GetList()).Where(it => it.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
