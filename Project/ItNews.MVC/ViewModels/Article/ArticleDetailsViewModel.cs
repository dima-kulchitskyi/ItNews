using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.Article
{
    public class ArticleDetailsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string AuthorName { get; set; }
        public string Date { get; set; }
        public bool HasImage { get; set; }
        public string Content { get; set; }
    }
}
