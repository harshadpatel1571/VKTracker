using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult DashBoard()
        {
            return View();
        }

        public async Task<ActionResult> Index()
        {
            var repository = new UserRepository();
            ViewBag.OrganizationDDL = new SelectList(await repository.BindUserOrganizationDDl(Convert.ToInt32(Session["userId"])), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult OrganizationSession(int? id)
        {
            var session = Session["OrganizationId"];
            if (session != null)
            {
                return Json(new { status = false });
            }
            else
            {
                Session["OrganizationId"] = id;
                return Json(new { status = true });
            }
        }
    }
}