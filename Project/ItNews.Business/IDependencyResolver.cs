using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business
{
    public interface IDependencyResolver
    {
         T Resolve<T>(string name = null);
    }
}
