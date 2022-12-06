using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VKTracker.Model.ViewModel;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    [Authorize]
    public class DistributionController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new DistributionViewModel();
            var repoItem = new ItemRepository();
            var repoFebric = new FabricRepository();
            var repoStockCode = new StockCodeRepository();
            var repoLocation = new LocationRepository();

            ViewBag.StockCodeDDL = new SelectList(await repoStockCode.BindStockCodeDDl(), "Id", "Name");
            ViewBag.FabricDDL = new SelectList(await repoFebric.BindFabricDDl(), "Id", "Name");
            ViewBag.ItemTypeDDL = new SelectList(await repoItem.BindItemDDl(), "Id", "Name");
            ViewBag.LocationDDL = new SelectList(await repoLocation.BindLocationDDl(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SaveDistributionList(DistributionViewModel objModel, List<int> StockIds)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }
            var repository = new DistributionRepository();
            var respose = await repository.SaveList(objModel, StockIds, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = true });
        }
    }
}