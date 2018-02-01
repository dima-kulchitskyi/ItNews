using ItNews.Business;
using ItNews.Web.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItNews.DependencyInjection
{
    public class DependencyResolver : IDependencyResolver
    {
        public T Resolve<T>(string name = null)
        {
            return (T)UnityConfiguration.Container.Resolve(typeof(T), name);
        }
    }
}