using System;

namespace ItNews.Business.Entities
{
    public class Article : IEntity
    {
        public virtual string Id { get; set; }
        public virtual AppUser Author { get; set; }
        public virtual string Title { get; set; }
        public virtual string ImageName { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
