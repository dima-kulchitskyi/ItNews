using ItNews.Business.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.Identity
{
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string PasswordHash { get; set; }
        public DateTimeOffset LockoutEndDate { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public IdentityUser Initialize(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PasswordHash = user.PasswordHash;
            LockoutEndDate = user.LockoutEndDate;
            LockoutEnabled = user.LockoutEnabled;
            AccessFailedCount = user.AccessFailedCount;
            Role = user.Role;
            return this;
        }

        public AppUser ToAppUser()
        {
            return new AppUser
            {
                Id = Id,
                UserName = UserName,
                Email = Email,
                PasswordHash = PasswordHash,
                LockoutEndDate = LockoutEndDate,
                LockoutEnabled = LockoutEnabled,
                AccessFailedCount = AccessFailedCount,
                Role = Role
            };
        }
    }
}
