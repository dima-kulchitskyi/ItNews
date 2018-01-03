using ItNews.MVC.Identity;
using ItNews.MVC.Identity.Mangers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ItNews
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user 
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider 
            // Configure the sign in cookie 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(WebConfigurationManager.AppSettings["IdentityLoginPath"]),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in. 
                    // This is a security feature which is used when you change a password or add an external login to your account.   
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<IdentityUserManager, IdentityUser>(
                        validateInterval: TimeSpan.FromMinutes(int.Parse(WebConfigurationManager.AppSettings["IdentityValidateInterval"])),
                        regenerateIdentity: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}