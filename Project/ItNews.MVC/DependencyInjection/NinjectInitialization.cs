using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace ItNews.Mvc.DependencyInjection
{
    public static class NinjectInitialization
    {
        public static void Initialize(INinjectModule module)
        {
            var container = new UnityContainer();

            var kernel = new StandardKernel(module);
            kernel.Unbind<ModelValidatorProvider>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
