using Ninject.Modules;
using ItNews.Nhibernate;
using Ninject.Web.Common;
using ItNews.Business.Providers;
using ItNews.Nhibernate.Providers;
using ItNews.Business.Entities;

namespace ItNews.Web
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            NhibernateConfiguration.RegisterDependencies(Kernel);

            Bind<SessionManager>().To<SessionManager>().InRequestScope();
            Bind(typeof(IProvider<>)).To(typeof(NhibernateProvider<>));
    
            Bind<IArticleProvider>().To<ArticleProvider>();
        }
    }
}