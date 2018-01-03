using Ninject.Modules;
using Ninject.Web.Common;
using ItNews.Business.Providers;
using ItNews.Business.Managers;
using ItNews.Business;
using System.Web;
using ItNews.MVC.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace ItNews.Web
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Nhibernate.Configuration.RegisterDependencies(Kernel);

            Bind<Nhibernate.SessionManager>().To<Nhibernate.SessionManager>().InRequestScope();

            Bind(typeof(IProvider<>)).To(typeof(Nhibernate.Providers.Provider<>));

            Bind<IUnitOfWorkFactory>().To<Nhibernate.UnitOfWorkFactory>();
            Bind<IUnitOfWork>().To<Nhibernate.UnitOfWork>();

            Bind<IArticleProvider>().To<Nhibernate.Providers.ArticleProvider>();
            Bind<IUserProvider>().To<Nhibernate.Providers.UserProvider>();

            //Identity
            Bind<IUserStore<IdentityUser, string>>().To<MVC.Identity.Stores.IdentityUserStore>();
            Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
            Bind<UserManager<IdentityUser, string>>().To<MVC.Identity.Mangers.IdentityUserManager>();
            Bind<Microsoft.AspNet.Identity.Owin.SignInManager<IdentityUser, string>>().To<MVC.Identity.Mangers.IdentitySignInManager>();
        }
    }
}