using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ItNews.MVC.ViewModels.Article
{
    public class DeleteViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
    }
}
