using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class ParcelRepository
    {
        public async Task<DataTableResponseCarrier<ParcelViewModel>> GetList(DataTableFilterViewModel filterDto, int userId, int organizationId, DateTime? fromDate, DateTime? toDate)
        {
            var db = new VKTrackerEntities();
            try
            {
                var result = db.ParcelReports.Where(x => x.IsActive && x.OrganizationId == organizationId &&
                ((fromDate.HasValue ? DbFunctions.TruncateTime(x.ArrivalDate.Value) >= DbFunctions.TruncateTime(fromDate) : true) &&
                (toDate.HasValue ? DbFunctions.TruncateTime(x.ArrivalDate.Value) <= DbFunctions.TruncateTime(toDate) : true))
                ).AsNoTracking().AsQueryable();//&& x.UserId == userId

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.ChalanNo.Contains(filterDto.SearchValue) ||
                                                x.Location.LocationName.Contains(filterDto.SearchValue) ||
                                                x.ParcelCode.Code.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<ParcelViewModel>
                {
                    TotalCount = result.Count()
                };
                result = result.OrderBy(x => x.Id);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new ParcelViewModel
                {
                    Id = x.Id,
                    ParcelId = x.ParcelId,
                    LocationId = x.LocatoinId,
                    ChallanNo = x.ChalanNo,
                    Code = x.ParcelCode.Code,
                    LocationName = x.Location.LocationName,
                    DishpatchDate = x.DispachedDate,
                    ArrivalDate = x.ArrivalDate
                });

                model.Data = await DynamicQueryableExtensions.OrderBy(response, filterDto.SortColumn + " " + filterDto.SortOrder).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

        public async Task<DataTableResponseCarrier<ParcelViewModel>> GetLogList(DataTableFilterViewModel filterDto, int id)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.ParcelReportLogs.Where(x => x.ParcelReportId == id).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelId.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<ParcelViewModel>
                {
                    TotalCount = result.Count()
                };

                result = result.OrderBy(x => x.Id);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new ParcelViewModel
                {
                    Id = x.Id,
                    ParcelId = (int)x.ParcelId,
                    LocationId = (int)x.LocationId,
                    ChallanNo = x.ChalanNo,
                    Code = db.ParcelCodes.FirstOrDefault(c => c.Id == x.ParcelId).Code,
                    LocationName = db.Locations.FirstOrDefault(c => c.Id == x.LocationId).LocationName,
                    DishpatchDate = x.DispachedDate,
                    ArrivalDate = x.ArrivalDate,
                    Action = (bool)x.IsActive ? x.Action : "delete",
                    CreatedOn = x.CreatedOn,
                    LogUserName = db.Users.FirstOrDefault(u => u.Id == x.CreatedBy).UserName,
                });

                response = DynamicQueryableExtensions.OrderBy(response, filterDto.SortColumn + " " + filterDto.SortOrder);
                model.Data = await response.ToListAsync().ConfigureAwait(false);

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

        public async Task<bool> Save(ParcelViewModel objModel, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new ParcelReport();

                if (objModel.Id > 0)
                {
                    model = await db.ParcelReports.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.ChalanNo = objModel.ChallanNo;
                model.LocatoinId = objModel.LocationId;
                model.ParcelId = objModel.ParcelId;
                model.ArrivalDate = objModel.ArrivalDate;
                model.DispachedDate = objModel.DishpatchDate;
                model.IsActive = true;

                if (organizationId > 0)
                {
                    model.OrganizationId = organizationId;
                }
                model.UserId = userId;

                if (objModel.Id > 0)
                {
                    model.Id = objModel.Id;
                    model.ModifiedBy = objModel.CreatedBy;
                    model.ModifiedOn = DateTime.UtcNow;
                    db.Entry(model).State = EntityState.Modified;
                }
                else
                {
                    model.CreatedBy = objModel.CreatedBy;
                    model.CreatedOn = DateTime.UtcNow;
                    db.ParcelReports.Add(model);
                }

                var status = await db.SaveChangesAsync().ConfigureAwait(false);
                return status > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
        }

        public async Task<ParcelViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.ParcelReports.Where(x => x.Id == id).Select(x => new ParcelViewModel
                {
                    Id = x.Id,
                    ChallanNo = x.ChalanNo,
                    LocationId = x.LocatoinId,
                    ParcelId = x.ParcelId,
                    ArrivalDate = x.ArrivalDate,
                    DishpatchDate = x.DispachedDate,
                }).FirstOrDefaultAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

        public async Task<bool> Delete(int id, int userId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.ParcelReports.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                model.IsActive = false;
                model.ModifiedBy = userId;
                model.ModifiedOn = DateTime.UtcNow;

                db.Entry(model).State = EntityState.Modified;
                var status = await db.SaveChangesAsync().ConfigureAwait(false);

                return status == 1 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db.Dispose();
            }
        }

        public async Task<List<ParcelViewModel>> Search(DateTime fromDate, DateTime toDate)
        {
            var db = new VKTrackerEntities();
            try
            {
                var lst = await db.ParcelReports.Where(x => x.DispachedDate >= fromDate.Date)
                    .Select(x => new ParcelViewModel
                    {
                        Id = x.Id,
                        ChallanNo = x.ChalanNo,
                        LocationId = x.LocatoinId,
                        ParcelId = x.ParcelId,
                        ArrivalDate = x.ArrivalDate,
                        DishpatchDate = x.DispachedDate,
                    }).ToListAsync().ConfigureAwait(false);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}
