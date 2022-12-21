using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
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
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel objModel)
        {
            if (string.IsNullOrEmpty(objModel.Tocken))
            {
                ModelState.AddModelError("", "captcha invalid try again.");
                return View(objModel);
            }
            else
            {
                bool status = await ValidateCaptcha(objModel.Tocken);
                if (!status)
                {
                    ModelState.AddModelError("", "captcha validation faild try again.");
                    return View(objModel);
                }
                else
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

                            if (userModel.IsAdmin)
                            {
                                return RedirectToAction("UserIndex", "Master");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "invalid Username or Password");
                            return View(objModel);
                        }
                    }
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

        public async Task<bool> ValidateCaptcha(string response)
        {
            string secretKey = ConfigurationManager.AppSettings["CaptchaSecretKey"];
            string apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}";

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(apiUrl);
                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponceViewModel>(json);
                    if (captchaResponse.Success)
                    {
                        return captchaResponse.Success;
                    }
                }
                return false;
            }
        }

    }
}