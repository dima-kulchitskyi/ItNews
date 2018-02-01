using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Mvc.ViewModels.Article
{
    public class CreateCommentViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string ArticleId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}