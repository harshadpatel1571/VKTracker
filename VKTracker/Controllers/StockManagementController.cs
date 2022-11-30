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
    public class StockManagementController : Controller
    {
        // GET: StockManagement
        public ActionResult Index()
        {
            return View();
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
            //bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Name);
            //if (isDuplicate)
            //{
            //    return Json(new { status = false, msg = "Duplicate Data Found !!" });
            //}

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }
    }
}