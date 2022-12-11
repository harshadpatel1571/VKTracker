using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Helper;
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
            var repoCustomer = new CustomerRepository();

            ViewBag.StockCodeDDL = new SelectList(await repoStockCode.BindStockCodeDDl(), "Id", "Name");
            ViewBag.FabricDDL = new SelectList(await repoFebric.BindFabricDDl(), "Id", "Name");
            ViewBag.ItemTypeDDL = new SelectList(await repoItem.BindItemDDl(), "Id", "Name");
            ViewBag.LocationDDL = new SelectList(await repoLocation.BindLocationDDl(), "Id", "Name");
            ViewBag.PartyDDl = new SelectList(await repoCustomer.BindCustomerDDl(), "Id", "Name");
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
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> GetDistributionList(int? stockCodeId, int? fabricId, int? itemTypeId, int? availableQuantity, int? locationId, string stockNo)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new DistributionRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]), stockCodeId, fabricId, itemTypeId, availableQuantity, locationId, stockNo).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<DistributionViewModel>
            {
                Draw = filter.Draw,
                Data = data.Data,
                RecordsFiltered = data.TotalCount,
                RecordsTotal = data.TotalCount
            };

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(responseModel, JsonSetting.Default),
                ContentEncoding = System.Text.Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDistributionList(List<DistributionViewModel> objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            objModel = objModel.Select(x => new DistributionViewModel
            {
                Id = x.Id,
                CreatedBy = Convert.ToInt32(Session["userId"]),
                CreatedOn = DateTime.Now,

            }).ToList();

            var repository = new DistributionRepository();
            var respose = await repository.DeleteList(objModel);
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> GetDistributionLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new DistributionRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<DistributionViewModel>
            {
                Draw = filter.Draw,
                Data = data.Data,
                RecordsFiltered = data.TotalCount,
                RecordsTotal = data.TotalCount
            };

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(responseModel, JsonSetting.Default),
                ContentEncoding = System.Text.Encoding.UTF8,
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDistribution(int id)
        {
            var repository = new DistributionRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> SaveDistribution(DistributionViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new DistributionRepository();
            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }

        [HttpGet]
        public async Task<ActionResult> EditDistribution(int id)
        {
            var repository = new DistributionRepository();
            var model = await repository.GetById(id);

            if (model != null)
            {
                return Json(new { status = true, data = model }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false });
            }
        }
    }
}