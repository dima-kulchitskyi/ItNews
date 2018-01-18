using FluentNHibernate.Mapping;
using ItNews.Business.Entities;

namespace ItNews.Nhibernate.Mappings
{
    class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Id(x => x.Id).GeneratedBy.Assigned();
            References(x => x.Author).Column("Author").Cascade.All().Not.Nullable().Not.LazyLoad();
            Map(x => x.Text).Not.Nullable();
            Map(x => x.Date).Not.Nullable();
            References(x => x.Article).Column("Article").Cascade.All().Not.Nullable().Not.LazyLoad();
        }
    }
}
