using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ItNews.MVC.ViewModels.News
{
    public class CreateViewModel
    {
        [Required]
        [MaxLength(255, ErrorMessage = "Ostanovites`")]
        public string Title { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
