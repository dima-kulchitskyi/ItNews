using FluentNHibernate.Mapping;
using ItNews.Business.Entities;

namespace ItNews.Nhibernate.Mappings
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(x => x.Id).GeneratedBy.Assigned();
            References(x => x.Author).Column("AuthorId").Cascade.None().Not.Nullable().Not.LazyLoad();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Text).CustomSqlType("ntext").Not.Nullable();
            Map(x => x.ImageName);
            Map(x => x.Date).Not.Nullable();
        }
    }
}
