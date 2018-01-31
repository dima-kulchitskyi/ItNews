using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Mvc.ViewModels.Article
{
    public class CommentViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { set; get; }

        [Display(Name = "Author")]
        public string Author { set; get; }

        [Display(Name = "Text")]
        public string Text { set; get; }

        [DataType(DataType.DateTime)]
        public DateTime Date { set; get; }
    }
}
