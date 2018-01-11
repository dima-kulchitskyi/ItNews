using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.Article
{
    public class CommentViewModel
    {
        public string Id { set; get; }
        public string Author { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
    }
}
