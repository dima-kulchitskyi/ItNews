using ItNews.Business;
using ItNews.Business.Providers;
using ItNews.DependencyInjection;
using ItNews.Mvc;
using ItNews.Mvc.Identity;
using MemoryProvider;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using Unity;
using Unity.Injection;

namespace ItNews.Web.DependencyInjection
{
    public static class UnityRegistrations
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            Nhibernate.Configuration.RegisterDependencies(container);

            container.RegisterType<IDependencyResolver, DependencyResolver>()

            //.RegisterType<IUnitOfWork, Nhibernate.UnitOfWork>()

            .RegisterType<IArticleProvider, Nhibernate.Providers.ArticleProvider>("DB")
            .RegisterType<IUserProvider, Nhibernate.Providers.UserProvider>("DB")
            .RegisterType<ICommentProvider, Nhibernate.Providers.CommentProvider>("DB")

            .RegisterType<IArticleProvider, ArticleProvider>("Memory")
            .RegisterType<IUserProvider, UserProvider>("Memory")
            .RegisterType<ICommentProvider, CommentProvider>("Memory")

            //Identity
            .RegisterType<IUserStore<IdentityUser, string>, Mvc.Identity.Stores.IdentityUserStore>()
            .RegisterType<IAuthenticationManager, IAuthenticationManager>(new InjectionFactory((c, t, s) => HttpContext.Current.GetOwinContext().Authentication))

            .RegisterType<UserManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentityUserManager>()
            .RegisterType<Microsoft.AspNet.Identity.Owin.SignInManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentitySignInManager>()

            .RegisterTypeInRequestScope<RequestDataStorage, RequestDataStorage>()
            .RegisterTypeInSingletonScope<ApplicationVariables, ApplicationVariables>();
        }
    }
}