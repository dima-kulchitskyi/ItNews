using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.News
{
    public class ArticlesListPageItem
    {
        public string Title { get; set; }
        public string UrlPath { get; set; }
        public string Author { get; set; }
        public string ImagePath { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
