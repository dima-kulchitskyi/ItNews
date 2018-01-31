using ItNews.Business;
using ItNews.Business.Providers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.Identity.Mangers
{
    public class IdentityUserManager : UserManager<IdentityUser, string>
    {
        public IdentityUserManager(IUserStore<IdentityUser, string> store) : base(store)
        {
        }        
    }
}
