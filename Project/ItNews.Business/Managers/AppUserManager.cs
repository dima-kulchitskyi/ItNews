using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class AppUserManager : Manager<AppUser>
    {
        private IUserProvider UserProvider;

        public AppUserManager(IUserProvider provider, IUnitOfWorkFactory unitOfWorkFactory) : base(provider, unitOfWorkFactory)
        {
            UserProvider = provider;
        }

        public Task<AppUser> GetUser(string id)
        {
            return UserProvider.Get(id);
        }

        public async Task DeleteAsync(AppUser user, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));

            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                uow.BeginTransaction();
                await UserProvider.Delete(user);
                uow.Commit();
            }
        }
        public Task<IList<AppUser>> GetAllUsers()
        {
            return UserProvider.GetAllUsers();
        }
    }
}
