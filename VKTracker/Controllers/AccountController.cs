using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;
using VKTracker.Repository;

namespace VKTracker.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel objModel)
        {
            using (var repository = new LoginRepository())
            {
                var userModel = await repository.ValidateUser(objModel);
                if (userModel != null)
                {
                    FormsAuthentication.SetAuthCookie(objModel.UserName, false);
                    Session["userId"] = userModel.Id;
                    Session["emailId"] = userModel.EmailId;
                    Session["isAdmin"] = userModel.IsAdmin;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "invalid Username or Password");
                    return View(objModel);
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["userId"] = null;
            Session["OrganizationId"] = null;
            return RedirectToAction("Login", "Account");
        }

    }
}