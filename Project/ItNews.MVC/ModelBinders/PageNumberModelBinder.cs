using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Mvc.ModelBinders
{
    public class PageNumberModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var name = bindingContext.ModelName;

            var pageNumberField = bindingContext.ValueProvider.GetValue(name);
            if (pageNumberField == null)
                return 0;

            var value = (int)pageNumberField?.ConvertTo(typeof(int)) - 1;

            return value > 0 ? value : 0;
        }
    }
}
