using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ItNews.Mvc.ModelBinders.Article
{
    public class ItemsOnPageCountModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var name = bindingContext.ModelName;
            var defaultItemsCount = int.Parse(WebConfigurationManager.AppSettings["NewsListItemsOnPageDefaultCount"]);

            var pageNumberField = bindingContext.ValueProvider.GetValue(name);

            if (pageNumberField == null)
                return defaultItemsCount;

            var value = (int)pageNumberField.ConvertTo(typeof(int));

            return value > 0 ? value : defaultItemsCount;
        }
    }
}
