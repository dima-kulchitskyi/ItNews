using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ItNews.Mvc.HtmlHelpers
{
    public static class HtmlAttributesExtensions
    {
        private static string FromDictionary(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null || htmlAttributes.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var pair in htmlAttributes)
                sb.Append(pair.Key).Append("=\"").Append(pair.Value).Append("\" ");

            return sb.ToString();
        }

        public static HtmlString HtmlAttributes(this HtmlHelper helper, object htmlAttributes)
        {
            if (htmlAttributes == null)
                return new HtmlString(string.Empty);

            return new HtmlString(FromDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }

        public static HtmlString HtmlAttributes(this HtmlHelper helper, Dictionary<string, object> htmlAttributes)
        {
            return new HtmlString(FromDictionary(htmlAttributes));
        }
    }
}
