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

namespace ItNews.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser, string> userManager;
        private SignInManager<IdentityUser, string> signInManager;
        private IAuthenticationManager authenticationManager;

        public AccountController(IdentityUserManager identityUserManager,
            IdentitySignInManager identitySignInManager,
            IAuthenticationManager identityAuthenticationManager)
        {
            userManager = identityUserManager;
            signInManager = identitySignInManager;
            authenticationManager = identityAuthenticationManager;
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

            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
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
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Article");
        }
    }
}