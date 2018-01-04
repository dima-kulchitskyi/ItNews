using ItNews.Business.Entities;
using ItNews.Business.Providers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.Identity.Stores
{
    public class IdentityUserStore : IUserStore<IdentityUser, string>, IUserPasswordStore<IdentityUser>, IUserLockoutStore<IdentityUser, string>, IUserTwoFactorStore<IdentityUser, string>
    {
        private IUserProvider userProvider;

        public IdentityUserStore(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public async Task CreateAsync(IdentityUser user)
        {
            var appUserInstance = user.ToAppUser();
            await  userProvider.SaveOrUpdate(appUserInstance);
            user.Id = appUserInstance.Id;
        }

        public Task DeleteAsync(IdentityUser user)
        {
            return userProvider.Delete(user.ToAppUser());
        }

        public void Dispose()
        {
            userProvider = null;
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            return new IdentityUser().Initialize(await userProvider.Get(userId));
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = await userProvider.GetByUserName(userName);
            return user != null ? new IdentityUser().Initialize(user) : null; 
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount = 0);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            return Task.FromResult(user.LockoutEnabled = enabled);
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(user.LockoutEndDate =  lockoutEnd);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(true);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            return Task.FromResult(true);
        }

        public Task UpdateAsync(IdentityUser user)
        {
            return userProvider.SaveOrUpdate(user.ToAppUser());
        }
    }
}
