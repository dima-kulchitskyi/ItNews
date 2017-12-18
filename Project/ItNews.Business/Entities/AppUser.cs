using System;

namespace ItNews.Business.Entities
{
    public class AppUser : IEntity
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
    }
}