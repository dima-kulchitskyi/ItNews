using ItNews.Business.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.MVC.Identity
{
    public class IdentityUser : IUser
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual DateTimeOffset LockoutEndDate { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

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
                AccessFailedCount = AccessFailedCount
            };
        }
    }
}
