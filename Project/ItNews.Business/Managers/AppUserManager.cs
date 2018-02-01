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
        public AppUserManager(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {   
        }

        public Task<AppUser> GetUser(string id)
        {
            return Provider.Get(id);
        }

        public async Task DeleteAsync(AppUser user, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = Provider.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await Provider.DeleteAsync(user);
                uow.Commit();
            }
        }

        public Task<IList<AppUser>> GetAllUsers()
        {
            return Provider.GetList();
        }
    }
}
