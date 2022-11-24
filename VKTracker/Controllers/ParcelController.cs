using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VKTracker.Model.ViewModel;

namespace VKTracker.Controllers
{
    [Authorize]
    public class ParcelController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Organization = new SelectList(new List<BindDropdownViewModel>(), "Id", "Name");
            return View();
        }
    }
}