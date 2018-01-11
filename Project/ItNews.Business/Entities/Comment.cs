using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Entities
{
    public class Comment : IEntity
    {
        public virtual string Id {set; get;}
        public virtual Article Article { set; get; }
        public virtual AppUser Author { set; get; }
        public virtual string Text { set; get; }
        public virtual DateTime Date { set; get; }
    }
}
