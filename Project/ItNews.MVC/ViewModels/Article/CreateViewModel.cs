using ItNews.Mvc.Attributes.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ItNews.Mvc.ViewModels.Article
{
    public class CreateViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [DataType(DataType.Upload)]
        [FileType("JPG,PNG")]

        public HttpPostedFileBase Image { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
