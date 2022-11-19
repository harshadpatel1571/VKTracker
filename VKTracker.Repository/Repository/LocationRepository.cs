using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class LocationRepository
    {
        public async Task<DataTableResponseCarrier<LocationViewModel>> GetList(DataTableFilterDto filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.Locations.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.LocationName.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<LocationViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new LocationViewModel
                {
                    Id = x.Id,
                    LocationName = x.LocationName
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

        public async Task<bool> Save(LocationViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new Location();

                if (objModel.Id > 0)
                {
                    model = await db.Locations.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.LocationName = objModel.LocationName;
                model.CreatedBy = objModel.CreatedBy;
                model.CreatedOn = DateTime.Now;
                model.IsActive = true;

                if (objModel.Id > 0)
                {
                    model.Id = objModel.Id;
                    db.Entry(model).State = EntityState.Modified;
                }
                else
                {
                    db.Locations.Add(model);
                }

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

        public async Task<bool> Delete(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.Locations.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                model.IsActive = false;

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

        public async Task<LocationViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Locations.Where(x => x.Id == id).Select(x => new LocationViewModel
                {
                    Id = x.Id,
                    LocationName = x.LocationName
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

        public async Task<bool> GetDuplicate(int id, string locationName)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Locations.AnyAsync(x => x.Id != id && x.LocationName.ToLower() == locationName.ToLower()).ConfigureAwait(false);
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
