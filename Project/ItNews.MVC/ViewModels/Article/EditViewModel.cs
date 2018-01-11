using ItNews.Mvc.CustomValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ItNews.Mvc.ViewModels.Article
{
    public class EditViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(255, ErrorMessage = "Ostanovites`")]
        public string Title { get; set; }

        [Display(Name = "Old Image")]
        [UIHint("Image")]
        public string OldImageName { get; set; }

        [Display(Name = "New Image")]
        [DataType(DataType.Upload)]
        [FileType("JPG,PNG")]
        public HttpPostedFileBase UploadedImage { get; set; }

        [Required]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
