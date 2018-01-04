using FluentNHibernate.Mapping;
using ItNews.Business.Entities;

namespace ItNews.Nhibernate.Mappings
{
    public class AppUserMap : ClassMap<AppUser>
    {
        public AppUserMap()
        {
            Table("Users");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Email).Not.Nullable();
            Map(x => x.UserName).Not.Nullable();
            Map(x => x.PasswordHash).Not.Nullable();

            Map(x => x.LockoutEndDate);
            Map(x => x.LockoutEnabled);
            Map(x => x.AccessFailedCount);
        }
    }
}
