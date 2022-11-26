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
    public class ParcelController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var parcelModel = new ParcelViewModel();
            var repoLocation = new LocationRepository();
            var repoParcelCode = new ParcelCodeRepository();
            ViewBag.LocationDDL = new SelectList(await repoLocation.BindLocationDDl(), "Id", "Name");
            ViewBag.ParcelDDL = new SelectList(await repoParcelCode.BindParcelDDl(), "Id", "Name");
            return View(parcelModel);
        }

        public async Task<ActionResult> GetParcelList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ParcelRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<ParcelViewModel>
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

        public async Task<ActionResult> GetParcelLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ParcelRepository();

            var data = await repository.GetLogList(filter,id).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<ParcelViewModel>
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
        public async Task<ActionResult> SaveParcelCode(ParcelViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new ParcelRepository();
            //bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Name);
            //if (isDuplicate)
            //{
            //    return Json(new { status = false, msg = "Duplicate Data Found !!" });
            //}

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel);
            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditParcel(int id)
        {
            var repository = new ParcelRepository();
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
        public async Task<ActionResult> DeleteParcel(int id)
        {
            var repository = new ParcelRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }
    }
}