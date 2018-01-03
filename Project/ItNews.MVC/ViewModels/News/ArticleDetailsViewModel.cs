using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.News
{
    public class ArticleDetailsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
    }
}
