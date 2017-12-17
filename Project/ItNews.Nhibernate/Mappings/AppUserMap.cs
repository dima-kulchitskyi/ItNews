using FluentNHibernate.Mapping;
using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Nhibernate.Mappings
{
    public class AppUserMap : ClassMap<AppUser>
    {
        public AppUserMap()
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.Email).Not.Nullable();
            Map(x => x.UserName);
            Map(x => x.PasswordHash).Not.Nullable();
        }
    }
}
