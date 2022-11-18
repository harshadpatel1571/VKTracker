using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    public class MasterController : Controller
    {
        #region Organization
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetOrganizationList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new OrganizationRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<OrganizationViewModel>
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
        public async Task<ActionResult> SaveOrganization(OrganizationViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new OrganizationRepository();
            var respose = await repository.Save(objModel);

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            var repository = new OrganizationRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditOrganization(int id)
        {
            var repository = new OrganizationRepository();
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

        #endregion

        #region Location Code
        public async Task<ActionResult> GetLocationList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new LocationRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<LocationViewModel>
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
        public async Task<ActionResult> SaveLocation(LocationViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new LocationRepository();
            var respose = await repository.Save(objModel);

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var repository = new LocationRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditLocation(int id)
        {
            var repository = new LocationRepository();
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

        #endregion

        #region Parcel

        public async Task<ActionResult> GetParcelCodeList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ParcelCodeRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<ParcelCodeViewModel>
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
        public async Task<ActionResult> SaveParcelCode(ParcelCodeViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new ParcelCodeRepository();
            var respose = await repository.Save(objModel);

            return Json(new { status = respose });
        }

        [HttpGet]
        public async Task<ActionResult> EditParcelCode(int id)
        {
            var repository = new ParcelCodeRepository();
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

        [HttpPost]
        public async Task<ActionResult> DeleteParcelCode(int id)
        {
            var repository = new ParcelCodeRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }
        #endregion
    }
}