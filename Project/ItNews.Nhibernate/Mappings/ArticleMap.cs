using FluentNHibernate.Mapping;
using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Nhibernate.Mappings
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(x => x.Id);
            HasOne(x => x.Author).Cascade.SaveUpdate().LazyLoad();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Text).Not.Nullable();
            Map(x => x.ImagePath);
            Map(x => x.Date).Not.Nullable();
        }
    }
}
