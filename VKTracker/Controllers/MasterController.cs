using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository;

namespace VKTracker.Controllers
{
    public class MasterController : Controller
    {
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
    }
}