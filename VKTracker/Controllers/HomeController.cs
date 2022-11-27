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
        public async Task<ActionResult> OrganizationSession(int? id)
        {
            if (!Convert.ToBoolean(Session["isAdmin"]))
            {
                var session = Session["OrganizationId"];
                if (session != null)
                {
                    return Json(new { status = false });
                }
                else
                {
                    var repository = new OrganizationRepository();
                    var data = await repository.GetById(Convert.ToInt32(id));
                    if (data != null)
                    {
                        Session["OrganizationId"] = data.Id;
                        Session["OrganizationName"] = data.Name;
                    }
                    return Json(new { status = true });
                }
            }
            return Json(new { status = false });
        }
    }
}