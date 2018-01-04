using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(ItNews.Startup))]
namespace ItNews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}