using ItNews.Business;
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

        private IUnitOfWorkFactory unitOfWorkFactory;

        public IdentityUserManager(IUserStore<IdentityUser, string> store, IUnitOfWorkFactory unitOfWorkFactory) : base(store)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
        public async override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            using (var uow = unitOfWorkFactory.GetUnitOfWork().BeginTransaction()) {
                var result = await base.CreateAsync(user, password);
                uow.Commit();
                return result;
            }
        }
    }
}
