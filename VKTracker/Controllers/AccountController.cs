using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VKTracker.Model.ViewModel;
using VKTracker.Repository;

namespace VKTracker.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel objModel)
        {
            using (var repository = new LoginRepository())
            {
                bool isValid = repository.ValidateUser(objModel);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(objModel.UserName, false);
                }
                else
                {
                    ModelState.AddModelError("", "invalid Username or Password");
                }
            }
            return RedirectToAction("Dashboard", "Home");
        }
    }
}