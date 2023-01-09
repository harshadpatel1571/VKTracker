using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class MasterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Organization = new SelectList(new List<BindDropdownViewModel>(), "Id", "Name");
            return View();
        }

        #region Organization

        public ActionResult OrganizationIndex()
        {
            return View();
        }

        public async Task<ActionResult> GetOrganizationList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new OrganizationRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Name, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            var repository = new OrganizationRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

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
            if (dropdownData != null)
            {
                return Json(new { status = true, data = dropdownData });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        public async Task<ActionResult> GetOrganizationLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new OrganizationRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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

        #endregion

        #region Location
        public ActionResult LocationIndex()
        {
            return View();
        }

        public async Task<ActionResult> GetLocationList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new LocationRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.LocationName, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var repository = new LocationRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

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

        public async Task<ActionResult> GetLocationLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new LocationRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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
        public async Task<ActionResult> BindLocationDropdown()
        {
            var repository = new LocationRepository();
            var dropdownData = new SelectList(await repository.BindLocationDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            if (dropdownData != null)
            {
                return Json(new { status = true, data = dropdownData });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        #endregion

        #region Parcel

        public ActionResult ParcelCodeIndex()
        {
            return View();
        }

        public async Task<ActionResult> GetParcelCodeList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ParcelCodeRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Code, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));

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
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }

        public async Task<ActionResult> GetParcelCodeLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ParcelCodeRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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
        public async Task<ActionResult> BindParcelDropdown()
        {
            var repository = new ParcelCodeRepository();
            var dropdownData = new SelectList(await repository.BindParcelDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            if (dropdownData != null)
            {
                return Json(new { status = true, data = dropdownData });
            }
            else
            {
                return Json(new { status = false });
            }
        }

        #endregion

        #region User

        public ActionResult UserIndex()
        {
            ViewBag.Organization = new SelectList(new List<BindDropdownViewModel>(), "Id", "Name");
            return View();
        }

        public async Task<ActionResult> GetUsersGrid()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new UserRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.FirstName, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.Password = Encryption.Encrypt(objModel.Password);
            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));

            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var repository = new UserRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

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

        public async Task<ActionResult> GetUserLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new UserRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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

        #endregion

        #region Fabric

        public ActionResult FabricIndex()
        {
            return View();
        }

        public async Task<ActionResult> GetFabricList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new FabricRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.FabricName, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {

                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFabric(int id)
        {
            var repository = new FabricRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

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


        public async Task<ActionResult> GetFabricLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new FabricRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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

        #endregion

        #region Item
        public ActionResult ItemIndex()
        {
            return View();
        }
        public async Task<ActionResult> GetItemList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ItemRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.ItemName, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));
            return Json(new { status = respose });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var repository = new ItemRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

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

        public async Task<ActionResult> GetItemLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new ItemRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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
        #endregion

        #region Stock
        public ActionResult StockCodeIndex()
        {
            return View();
        }
        public async Task<ActionResult> GetStockCodeList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockCodeRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

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
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Code, Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));

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
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }

        public async Task<ActionResult> GetStockCodeLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new StockCodeRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

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
        #endregion

        #region Customer

        public async Task<ActionResult> CustomerIndex()
        {
            var repoLocation = new LocationRepository();
            ViewBag.LocationDDL = new SelectList(await repoLocation.BindLocationDDl(Convert.ToInt32(Session["OrganizationId"])), "Id", "Name");
            return View();
        }

        public async Task<ActionResult> GetCustomerList()
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new CustomerRepository();

            var data = await repository.GetList(filter, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"])).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<CustomerViewModel>
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
        public async Task<ActionResult> SaveCustomer(CustomerViewModel objModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model is not valid");
            }

            var repository = new CustomerRepository();
            bool isDuplicate = await repository.GetDuplicate(objModel.Id, objModel.Name,Convert.ToInt32(Session["OrganizationId"]));
            if (isDuplicate)
            {
                return Json(new { status = false, msg = "Duplicate Data Found !!" });
            }

            objModel.CreatedBy = Convert.ToInt32(Session["userId"]);
            var respose = await repository.Save(objModel, Convert.ToInt32(Session["userId"]), Convert.ToInt32(Session["OrganizationId"]));

            return Json(new { status = respose });
        }

        [HttpGet]
        public async Task<ActionResult> EditCustomer(int id)
        {
            var repository = new CustomerRepository();
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
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var repository = new CustomerRepository();
            var respose = await repository.Delete(id, Convert.ToInt32(Session["userId"]));

            return Json(new { status = respose });
        }

        public async Task<ActionResult> GetCustomerLogList(int id)
        {
            var filter = DataExtractor.Extract(Request);

            var repository = new CustomerRepository();

            var data = await repository.GetLogList(filter, id).ConfigureAwait(false);

            var responseModel = new DataTableResponseDto<CustomerViewModel>
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


        #endregion
    }
}