using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace ItNews.MVC.Identity.Mangers
{
    public class IdentitySignInManager : SignInManager<IdentityUser, string>
    {
        public IdentitySignInManager(UserManager<IdentityUser, string> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}
