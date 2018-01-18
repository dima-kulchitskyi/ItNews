using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IUserProvider : IProvider<AppUser>
    {
        Task<AppUser> GetByUserName(string userName);
        Task<IList<AppUser>> GetAllUsers();

    }
}
