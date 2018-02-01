using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;
using ItNews.Business.Caching;

namespace ItNews.Business.Managers
{
    public class AppUserManager : Manager<AppUser, IUserProvider, CacheProvider<AppUser>>
    {
        public AppUserManager(IUserProvider provider, CacheProvider<AppUser> cacheProvider) : base(provider, cacheProvider)
        {
            
        }

        public Task<AppUser> GetUser(string id)
        {
            return provider.Get(id);
        }

        public async Task DeleteAsync(AppUser user, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = provider.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await provider.Delete(user);
                uow.Commit();
            }

            cacheProvider.Clear(user.Id);
        }

        public Task<IList<AppUser>> GetAllUsers()
        {
            return provider.GetAllUsers();
        }
    }
}
