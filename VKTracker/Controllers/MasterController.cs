using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using VKTracker.Common.Helper;
using VKTracker.Helper;
using VKTracker.Model.ViewModel;
using VKTracker.Repository;
using VKTracker.Repository.Repository;

namespace VKTracker.Controllers
{
    public class MasterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Organization = new SelectList(new List<BindDropdownViewModel>(), "Id", "Name");
            return View();
        }

        #region Organization
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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Name);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
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

        [HttpPost]
        public async Task<ActionResult> BindOrganizationDropdown()
        {
            var repository = new OrganizationRepository();
            var dropdownData = new SelectList(await repository.BindOrganizationDDl(), "Id", "Name");
            if(dropdownData != null)
            {
                return Json(new { status = true, data = dropdownData });
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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.LocationName);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Code);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
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

        #region User
        public async Task<ActionResult> GetUsersGrid()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new UserRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<UserViewModel>
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
        public async Task<ActionResult> SaveUser(UserViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new UserRepository();
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.FirstName);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.Password = Encryption.Encrypt(objModel.Password);
            var respose = await repository.Save(objModel);

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var repository = new UserRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditUser(int id)
        {
            var repository = new UserRepository();
            var model = await repository.GetById(id);
            model.Password = Encryption.Decrypt(model.Password);

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

        #region Fabric

        public async Task<ActionResult> GetFabricList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new FabricRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<FabricViewModel>
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
        public async Task<ActionResult> SaveFabric(FabricViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new FabricRepository();
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.FabricName);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
            var respose = await repository.Save(objModel);
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFabric(int id)
        {
            var repository = new FabricRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditFabric(int id)
        {
            var repository = new FabricRepository();
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

        #region Item

        public async Task<ActionResult> GetItemList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ItemRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<ItemViewModel>
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
        public async Task<ActionResult> SaveItem(ItemViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new ItemRepository();
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.ItemName);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
            var respose = await repository.Save(objModel);
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var repository = new ItemRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }

        public async Task<ActionResult> EditItem(int id)
        {
            var repository = new ItemRepository();
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

        #region Stock

        public async Task<ActionResult> GetStockCodeList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockCodeRepository();

            var data = await repository.GetList(filter).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<StockCodeViewModel>
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
        public async Task<ActionResult> SaveStockCode(StockCodeViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new StockCodeRepository();
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Code);
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }
            var respose = await repository.Save(objModel);

            return Json(new { status = respose });
        }

        [HttpGet]
        public async Task<ActionResult> EditStockCode(int id)
        {
            var repository = new StockCodeRepository();
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
        public async Task<ActionResult> DeleteStockCode(int id)
        {
            var repository = new StockCodeRepository();
            var respose = await repository.Delete(id);

            return Json(new { status = respose });
        }
        #endregion
    }
}