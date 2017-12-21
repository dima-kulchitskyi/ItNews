using Ninject.Modules;
using Ninject.Web.Common;
using ItNews.Business.Providers;
using ItNews.Business.Managers;
using ItNews.Business;

namespace ItNews.Mvc.DependencyInjection
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Nhibernate.Configuration.RegisterDependencies(Kernel);

            Bind<Nhibernate.SessionManager>().To<Nhibernate.SessionManager>().InRequestScope();
            Bind<IUnitOfWork>().To<Nhibernate.UnitOfWork>();

            Bind(typeof(IProvider<>)).To(typeof(Nhibernate.Providers.Provider<>));

            Bind<IArticleProvider>().To<Nhibernate.Providers.ArticleProvider>();
            Bind<IAppUserProvider>().To<Nhibernate.Providers.AppUserProvider>();

            Bind<AppUserManager>().To<AppUserManager>();
            Bind<ArticleManager>().To<ArticleManager>();
        }
    }
}