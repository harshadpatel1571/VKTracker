using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class UserRepository
    {
        public async Task<DataTableResponseCarrier<UserViewModel>> GetList(DataTableFilterDto filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.Users.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.FirstName.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<UserViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    EmailId = x.EmailId,
                    MobileNo = x.MobileNo,
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

        public async Task<bool> Save(UserViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new User();

                if (objModel.Id > 0)
                {
                    model = await db.Users.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.FirstName = objModel.FirstName;
                model.LastName = objModel.LastName;
                model.UserName = objModel.UserName;
                model.Password = objModel.Password;
                model.EmailId = objModel.EmailId;
                model.MobileNo = objModel.MobileNo;
                model.CreatedBy = objModel.CreatedBy;
                model.CreatedOn = DateTime.Now;
                model.IsActive = true;
                
                if (objModel.Id > 0)
                {
                    model.Id = objModel.Id;
                    var old = db.UserOrganizations.Where(x=>x.UserId == objModel.Id).ToList();
                    db.UserOrganizations.RemoveRange(old);

                    var newUserOrganization = objModel.OrganizationId.Select(x => new UserOrganization
                    {
                        UserId = objModel.Id,
                        OrganizationId = x
                    }).ToArray();
                    db.UserOrganizations.AddRange(newUserOrganization);

                    db.Entry(model).State = EntityState.Modified;
                }
                else
                {
                    model.UserOrganizations = objModel.OrganizationId.Select(x => new UserOrganization
                    {
                        OrganizationId = x
                    }).ToArray();
                    db.Users.Add(model);
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

        public async Task<bool> Delete(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<UserViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Users.Where(x => x.Id == id).Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Password = x.Password,
                    EmailId = x.EmailId,
                    OrganizationId = (IList<int>)x.UserOrganizations.Select(y=>y.OrganizationId)
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

        public async Task<bool> GetDuplicate(int id, string name)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Users.AnyAsync(x => x.Id != id && x.UserName.ToLower() == name.ToLower()).ConfigureAwait(false);
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
