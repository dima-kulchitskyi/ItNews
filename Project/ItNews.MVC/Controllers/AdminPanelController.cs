using ItNews.Business.Managers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ItNews.Mvc.Controllers
{
    public class AdminPanelController : Controller
    {
        AppUserManager appUserManager;

        public AdminPanelController(AppUserManager appUserManager)
        {
            this.appUserManager = appUserManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Panel()
        {
            var users = await appUserManager.GetAllUsers();
            var model = new ViewModels.AdminPanel.PanelViewModel
            {
                UserName = users
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = await appUserManager.GetUser(id);
            
            await appUserManager.DeleteAsync(user, id);

            return RedirectToAction("Panel");
        }

        //[HttpGet]
        //public async Task<ActionResult> Edit(string id)
        //{
        //    var user = await appUserManager.GetUser(id);
        //    if (user != null)
        //    {
        //        return View(user);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(string id, string email, string password)
        //{
        //    AppUser user = await appUserManager.GetUser(id);
        //    if (user != null)
        //    {
        //        user.Email = email;
        //        IdentityResult validEmail
        //            = await appUserManager.UserValidator.ValidateAsync(user);

        //        if (!validEmail.Succeeded)
        //        {
        //            AddErrorsFromResult(validEmail);
        //        }

        //        IdentityResult validPass = null;
        //        if (password != string.Empty)
        //        {
        //            validPass
        //                = await appUserManager.PasswordValidator.ValidateAsync(password);

        //            if (validPass.Succeeded)
        //            {
        //                user.PasswordHash =
        //                    appUserManager.PasswrdHasher.HashPassword(password);
        //            }
        //            else
        //            {
        //                AddErrorsFromResult(validPass);
        //            }
        //        }

        //        if ((validEmail.Succeeded && validPass == null) ||
        //                (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
        //        {
        //            IdentityResult result = await appUserManager.UpdateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                AddErrorsFromResult(result);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Пользователь не найден");
        //    }
        //    return View(user);
        //}

    }
}
