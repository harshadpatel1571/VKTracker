using System;
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
        public async Task<DataTableResponseCarrier<ParcelViewModel>> GetList(DataTableFilterViewModel filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.ParcelReports.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelCode.Code.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<ParcelViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new ParcelViewModel
                {
                    Id = x.Id,
                    ParcelId = x.ParcelId,
                    LocationId = x.LocatoinId,
                    ChallanNo = x.ChalanNo,
                    Code=x.ParcelCode.Code,
                    LocationName = x.Location.LocationName,
                    DishpatchDate = x.DispachedDate,
                    ArrivalDate = x.ArrivalDate
                }).ToListAsync().ConfigureAwait(false);

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
                var result = db.ParcelReportLogs.Where(x => x.ParcelId == id).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelId.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<ParcelViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new ParcelViewModel
                {
                    Id = x.Id,
                    ParcelId = (int)x.ParcelId,
                    LocationId = (int)x.LocationId,
                    ChallanNo = x.ChalanNo,
                    DishpatchDate = x.DispachedDate,
                    ArrivalDate = x.ArrivalDate,
                    Action = (bool)x.IsActive ? x.Action : "delete",
                    CreatedOn = x.CreatedOn,
                    LogUserName = db.Users.FirstOrDefault(u => u.Id == x.CreatedBy).UserName,
                }).ToListAsync().ConfigureAwait(false);

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
    }
}
