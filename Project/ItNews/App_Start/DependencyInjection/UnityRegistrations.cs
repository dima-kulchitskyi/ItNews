using ItNews.Business;
using ItNews.Business.Providers;
using ItNews.Mvc.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using Unity;
using Unity.Injection;

namespace ItNews.Web.DependencyInjection
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityRegistrations
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            Nhibernate.Configuration.RegisterDependencies(container);

            container
            .RegisterTypeInRequestScope<Nhibernate.SessionManager, Nhibernate.SessionManager>()

            .RegisterType<IUnitOfWorkFactory, Nhibernate.UnitOfWorkFactory>()
            .RegisterType<IUnitOfWork, Nhibernate.UnitOfWork>()

            .RegisterType<IArticleProvider, Nhibernate.Providers.ArticleProvider>()
            .RegisterType<IUserProvider, Nhibernate.Providers.UserProvider>()

            //Identity
            .RegisterType<IUserStore<IdentityUser, string>, Mvc.Identity.Stores.IdentityUserStore>()
            .RegisterType<IAuthenticationManager, IAuthenticationManager>(new InjectionFactory((c, t, s) => HttpContext.Current.GetOwinContext().Authentication))

            .RegisterType<UserManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentityUserManager>()
            .RegisterType<Microsoft.AspNet.Identity.Owin.SignInManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentitySignInManager>();
        }
    }
}