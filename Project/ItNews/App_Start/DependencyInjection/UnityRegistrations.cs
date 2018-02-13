using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using ItNews.Business.Search;
using ItNews.DependencyInjection;
using ItNews.Mvc;
using ItNews.Mvc.Identity;
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

            .RegisterType<IArticleProvider, Nhibernate.Providers.ArticleProvider>("DB")
            .RegisterType<IUserProvider, Nhibernate.Providers.UserProvider>("DB")
            .RegisterType<ICommentProvider, Nhibernate.Providers.CommentProvider>("DB")

            .RegisterType<IArticleProvider, FileProvider.ArticleProvider>("File")
            .RegisterType<IUserProvider, FileProvider.UserProvider>("File")
            .RegisterType<ICommentProvider, FileProvider.CommentProvider>("File")

            .RegisterType<IArticleSearchProvider, SearchProvider.ArticleProvider>()

            //Identity
            .RegisterType<IUserStore<IdentityUser, string>, Mvc.Identity.Stores.IdentityUserStore>()
            .RegisterType<IAuthenticationManager, IAuthenticationManager>(new InjectionFactory((c, type, name) => HttpContext.Current.GetOwinContext().Authentication))

            .RegisterType<UserManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentityUserManager>()
            .RegisterType<Microsoft.AspNet.Identity.Owin.SignInManager<IdentityUser, string>, Mvc.Identity.Mangers.IdentitySignInManager>()
            
            ;
        }
    }
}