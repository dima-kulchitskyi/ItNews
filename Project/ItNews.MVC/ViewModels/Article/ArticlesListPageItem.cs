using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.Article
{
    public class ArticlesListPageItem
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Author { get; set; }
        public string ImageName { get; set; }
        public string TextPreview { get; set; }
        public DateTime Date { get; set; }
    }
}
