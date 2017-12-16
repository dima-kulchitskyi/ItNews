using ItNews.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Entities
{
    public class Article : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual AppUser Author { get; set; }
        public virtual string Title { get; set; }
        public virtual string Image { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
