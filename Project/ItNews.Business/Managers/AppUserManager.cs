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
        public AppUserManager(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {   
        }

        public async Task SaveOrUpdate(AppUser user)
        {
            using (var uow = provider.GetUnitOfWork())
                 await provider.SaveOrUpdate(user);

            cacheProvider.Clear(user.Id);
        }

        public async Task Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = await base.GetById(userId);

            if (user == null)
                throw new InvalidOperationException("No such user");

            await Delete(user);

            cacheProvider.Clear(user.Id);
        }

        public Task<AppUser> GetByName(string name)
        {
            return provider.GetByUserNameAsync(name);
        }

        public async Task Delete(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

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
            return provider.GetList();
        }
    }
}
