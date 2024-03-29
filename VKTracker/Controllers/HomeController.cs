﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Common.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AuthorizeActionFilter]
        public async Task<ActionResult> DashBoard()
        {
            var model = new DistributionViewModel();
            
            var repoCustomer = new CustomerRepository();
            var repoLocation = new LocationRepository();

            ViewBag.PartyDDl = new SelectList(await repoCustomer.BindCustomerDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.LocationDDL = new SelectList(await repoLocation.BindLocationDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");

            return View(model);
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