using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository
{
    public class OrganizationRepository
    {
        public async Task<DataTableResponseCarrier<OrganizationViewModel>> GetList(DataTableFilterDto filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.Organizations.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<OrganizationViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new OrganizationViewModel
                {
                    Id = x.Id,
                    Name = x.Name
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

        public async Task<bool> Save(OrganizationViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new Organization();

                if (objModel.Id > 0)
                {
                    model = await db.Organizations.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.Name = objModel.Name;
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
                    db.Organizations.Add(model);
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
                var model = await db.Organizations.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<OrganizationViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Organizations.Where(x => x.Id == id).Select(x => new OrganizationViewModel
                {
                    Id = x.Id,
                    Name = x.Name
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
    }
}
