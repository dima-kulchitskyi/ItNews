using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.Article
{
    public class CreateCommentViewModel
    {
        public string ArticleId { get; set; }
        public string Text { get; set; }
    }
}