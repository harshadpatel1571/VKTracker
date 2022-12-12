using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VKTracker.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    [Authorize]
    public class StockManagementController : Controller
    {
        // GET: StockManagement
        public async Task<ActionResult> Index()
        {
            var model = new StockManagementViewModel();
            var repoParcelCode = new ParcelCodeRepository();
            var repoItem = new ItemRepository();
            var repoFebric = new FabricRepository();
            var repoStockCode = new StockCodeRepository();
            var repoLocation = new LocationRepository();
            var repoCustomer = new CustomerRepository();

            ViewBag.ParcelDDL = new SelectList(await repoParcelCode.BindParcelDDl(), "Id", "Name");
            ViewBag.StockCodeDDL = new SelectList(await repoStockCode.BindStockCodeDDl(), "Id", "Name");
            ViewBag.LocationDDl = new SelectList(await repoLocation.BindLocationDDl(), "Id", "Name");
            ViewBag.FabricDDl = new SelectList(await repoFebric.BindFabricDDl(), "Id", "Name");
            ViewBag.ItemDDl = new SelectList(await repoItem.BindItemDDl(), "Id", "Name");
            ViewBag.PartyDDl = new SelectList(await repoCustomer.BindCustomerDDl(), "Id", "Name");
            return View(model);
        }

        public async Task<ActionResult> GetStockManagementList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockManagementRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<StockManagementViewModel>
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
        public async Task<ActionResult> DeleteStockManagement(int id)
        {
            var repository = new StockManagementRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> GetStockManagementLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockManagementRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<StockManagementViewModel>
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

        public async Task<ActionResult> EditStockManagement(int id)
        {
            var repository = new StockManagementRepository();
            var model = await repository.GetById(id);

            if (model != null)
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { status = true, data = model }, JsonSetting.Default),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json"
                };
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveStockManagement(StockManagementViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new StockManagementRepository();
            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> SaveStockManagementList(StockManagementListModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            objModel.StockManagementList = objModel.StockManagementList.Select(x => new StockManagementViewModel
            {
                ParcelId = x.ParcelId,
                StockCodeId = x.StockCodeId,
                FabricId = x.FabricId,
                ItemId = x.ItemId,
                LocationId = x.LocationId,
                TotalQuantity = x.TotalQuantity,
                CreatedBy = Convert.ToInt32(Session["userId"]),
                CreatedOn = DateTime.Now,
                UserId = Convert.ToInt32(Session["userId"]),
                OrganizationId = Convert.ToInt32(Session["OrganizationId"])
            }).ToList();

            var repository = new StockManagementRepository();
            var respose = await repository.SaveList(objModel.StockManagementList);
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStockManagementList(StockManagementListModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            objModel.StockManagementList = objModel.StockManagementList.Select(x => new StockManagementViewModel
            {
                Id = x.Id,
                CreatedBy = Convert.ToInt32(Session["userId"]),
                CreatedOn = DateTime.Now,

            }).ToList();

            var repository = new StockManagementRepository();
            var respose = await repository.DeleteList(objModel.StockManagementList);
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> TransferLocation(StockManagementListModel objModel, int locationId)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            objModel.StockManagementList = objModel.StockManagementList.Select(x => new StockManagementViewModel
            {
                Id = x.Id,
                CreatedBy = Convert.ToInt32(Session["userId"]),
                CreatedOn = DateTime.Now,

            }).ToList();

            var repository = new StockManagementRepository();
            var respose = await repository.TransferLocation(objModel.StockManagementList, locationId);
            return Json(new { status = respose });
        }
    }
}