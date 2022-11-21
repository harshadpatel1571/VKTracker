using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class ParcelCodeRepository
    {
        public async Task<DataTableResponseCarrier<ParcelCodeViewModel>> GetList(DataTableFilterViewModel filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.ParcelCodes.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.Code.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<ParcelCodeViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new ParcelCodeViewModel
                {
                    Id = x.Id,
                    Code = x.Code
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

        public async Task<bool> Save(ParcelCodeViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new ParcelCode();

                if (objModel.Id > 0)
                {
                    model = await db.ParcelCodes.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.Code = objModel.Code;
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
                    db.ParcelCodes.Add(model);
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


        public async Task<ParcelCodeViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.ParcelCodes.Where(x => x.Id == id).Select(x => new ParcelCodeViewModel
                {
                    Id = x.Id,
                    Code = x.Code
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

        public async Task<bool> Delete(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.ParcelCodes.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<bool> GetDuplicate(int id, string code)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.ParcelCodes.AnyAsync(x => x.Id != id && x.Code.ToLower() == code.ToLower()).ConfigureAwait(false);
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
