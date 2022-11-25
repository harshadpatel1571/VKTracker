using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index()
        {
            ViewBag.Organization = new SelectList(new List<BindDropdownViewModel>(), "Id", "Name");
            return View();
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
    }
}