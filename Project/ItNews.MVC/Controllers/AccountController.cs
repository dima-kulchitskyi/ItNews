using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ItNews.Mvc.ViewModels.Account;
using ItNews.Business.Entities;
using ItNews.Business.Managers;
using ItNews.Mvc.Identity.Mangers;
using ItNews.Mvc.Identity;
using System.Threading;
using ItNews.Mvc.ViewModels.UserProfile;
using System.Web.UI;
using ItNews.Business;

namespace ItNews.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private AppUserManager userManager;

        private UserManager<IdentityUser, string> identityUserManager;

        private SignInManager<IdentityUser, string> identitySignInManager;

        private IAuthenticationManager identityAuthenticationManager;

        public AccountController(AppUserManager userManager,
            IdentityUserManager identityUserManager,
            IdentitySignInManager identitySignInManager,
            IAuthenticationManager identityAuthenticationManager)
        {
            this.userManager = userManager;
            this.identityUserManager = identityUserManager;
            this.identitySignInManager = identitySignInManager;
            this.identityAuthenticationManager = identityAuthenticationManager;
        }

        public async Task<ActionResult> Test()
        {
            var thread1 = Thread.CurrentThread;
            Thread.SetData(Thread.GetNamedDataSlot("123"), "DIMAS");

            var result2 = (string)Thread.GetData(Thread.GetNamedDataSlot("123"));

            bool equals = true;
            int count = 0;
            while (equals)
            {
                await Task.Run(() => Thread.Sleep(200));
                var thread2 = Thread.CurrentThread;

                equals = thread1 == thread2;
                count++;
            }
            var result = (string)Thread.GetData(Thread.GetNamedDataSlot("123"));
            return Content(equals.ToString() + " " + count);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await identitySignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Article");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email, Role = "User" };
                var result = await identityUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await identitySignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Article");
                }

                foreach (var error in result.Errors)    
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            identityAuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Article");
        }

        [HttpGet]
        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any)]
        public async Task<ActionResult> UserProfile()
        {
            var user = await userManager.GetById(User.Identity.GetUserId());

            if (user == null)
                return HttpNotFound();

            return View(new UserProfileViewModel() { UserName = user.UserName, Email = user.Email });
        }
    }
}