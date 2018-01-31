using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ItNews.Mvc.CustomValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string defaultErrorMessage = "Only the following file types are allowed: {0}";
        private IEnumerable<string> validExtensions;

        public FileTypeAttribute(string validTypes)
        {
            validExtensions = validTypes.Split(',', ';', '|').Select(s => s.Trim().ToLower());
            ErrorMessage = ErrorMessage ?? string.Format(defaultErrorMessage, string.Join(", ", validExtensions));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<HttpPostedFileBase> files)
            {
                foreach (var file in files)
                {
                    if (file != null && !validExtensions.Any(e => file.FileName.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
                        return new ValidationResult(ErrorMessageString);
                }
            }
            else
            {
                if (value is HttpPostedFileBase file)
                {
                    if (!validExtensions.Any(e => file.FileName.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
                        return new ValidationResult(ErrorMessageString);
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters.Add("validtypes", string.Join(",", validExtensions));

            return new ModelClientValidationRule[] { rule };
        }
    }
}
