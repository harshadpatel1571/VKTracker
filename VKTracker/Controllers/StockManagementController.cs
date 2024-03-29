﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Common.Helper;
using VKTracker.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    [Authorize]
    [AuthorizeActionFilter]
    public class StockManagementController : Controller
    {
        // GET: StockManagement
        public async Task<ActionResult> Index()
        {
            var model = new StockManagementViewModel();
            //var repoParcelCode = new ParcelCodeRepository();
            var repoParcel = new ParcelRepository();
            var repoItem = new ItemRepository();
            var repoFebric = new FabricRepository();
            var repoStockCode = new StockCodeRepository();
            var repoLocation = new LocationRepository();
            var repoCustomer = new CustomerRepository();

            //ViewBag.ParcelDDL = new SelectList(await repoParcelCode.BindParcelDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.ParcelDDL = new SelectList(await repoParcel.BindParcelReportDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.StockCodeDDL = new SelectList(await repoStockCode.BindStockCodeDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.LocationDDl = new SelectList(await repoLocation.BindLocationDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.FabricDDl = new SelectList(await repoFebric.BindFabricDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.ItemDDl = new SelectList(await repoItem.BindItemDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            ViewBag.PartyDDl = new SelectList(await repoCustomer.BindCustomerDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetStockManagementList(int? stockCodeId, int? fabricId, int? itemTypeId, int? availableQuantity, int? locationId, int? stockNo, DateTime? fromDate, DateTime? toDate)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockManagementRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]), stockCodeId, fabricId, itemTypeId, availableQuantity, locationId, stockNo, fromDate, toDate).ConfigureAwait(false);

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

            //objModel.StockManagementList = objModel.StockManagementList.Select(x => new StockManagementViewModel
            //{
            //    ParcelId = x.ParcelId,
            //    StockCodeId = x.StockCodeId,
            //    FabricId = x.FabricId,
            //    ItemId = x.ItemId,
            //    LocationId = x.LocationId,
            //    TotalQuantity = x.TotalQuantity,
            //    CreatedBy = Convert.ToInt32(Session["userId"]),
            //    CreatedOn = DateTime.UtcNow,
            //    //UserId = Convert.ToInt32(Session["userId"]),
            //    //OrganizationId = Convert.ToInt32(Session["OrganizationId"])
            //}).ToList();


            var listModel = new List<StockManagementViewModel>();

            foreach (var item in objModel.StockManagementList)
            {
                foreach (var than in item.ThanList)
                {
                    var m = new StockManagementViewModel
                    {
                        ParcelId = objModel.ParcelId,
                        StockCodeId = item.StockCodeId,
                        FabricId = item.FabricId,
                        ItemId = item.ItemId,
                        LocationId = item.LocationId,
                        TotalQuantity = than,
                        CreatedBy = Convert.ToInt32(Session["userId"]),
                        CreatedOn = DateTime.UtcNow,
                    };
                    listModel.Add(m);
                }
            }

            

            var repository = new StockManagementRepository();
            //var respose = await repository.SaveList(objModel.StockManagementList, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            var respose = await repository.SaveList(listModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
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
                CreatedOn = DateTime.UtcNow,

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
                CreatedOn = DateTime.UtcNow,

            }).ToList();

            var repository = new StockManagementRepository();
            var respose = await repository.TransferLocation(objModel.StockManagementList, locationId);
            return Json(new { status = respose });
        }
    }
}