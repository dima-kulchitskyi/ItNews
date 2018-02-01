using FluentNHibernate.Mapping;
using ItNews.Business.Entities;

namespace ItNews.Nhibernate.Mappings
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Id(x => x.Id).GeneratedBy.Assigned();
            References(x => x.Author).Column("AuthorId").Cascade.None().Not.Nullable().Not.LazyLoad();
            Map(x => x.Text).CustomSqlType("ntext").Not.Nullable();
            Map(x => x.Date).Not.Nullable();
            References(x => x.Article).Column("ArticleId").Cascade.None().Not.Nullable().Not.LazyLoad();
        }
    }
}
