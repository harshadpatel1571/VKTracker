﻿using System;
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
    public class CustomerRepository
    {
        public async Task<DataTableResponseCarrier<CustomerViewModel>> GetList(DataTableFilterViewModel filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.Customers.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.Name.Contains(filterDto.SearchValue) ||
                                          x.Mobile.Contains(filterDto.SearchValue) ||
                                          x.Address.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<CustomerViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new CustomerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address= x.Address,
                    Mobile = x.Mobile

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

        public async Task<bool> Save(CustomerViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new Customer();

                if (objModel.Id > 0)
                {
                    model = await db.Customers.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.Name = objModel.Name;
                model.Address = objModel.Address;
                model.Mobile = objModel.Mobile;
                model.IsActive = true;

                if (objModel.Id > 0)
                {
                    model.Id = objModel.Id;
                    model.ModifiedBy = objModel.CreatedBy;
                    model.ModifiedOn = DateTime.Now;
                    db.Entry(model).State = EntityState.Modified;
                }
                else
                {
                    model.CreatedBy = objModel.CreatedBy;
                    model.CreatedOn = DateTime.Now;
                    db.Customers.Add(model);
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

        public async Task<bool> Delete(int id, int userId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.Customers.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                model.IsActive = false;
                model.ModifiedBy = userId;
                model.ModifiedOn = DateTime.Now;

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

        public async Task<CustomerViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Customers.Where(x => x.Id == id).Select(x => new CustomerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address= x.Address,
                    Mobile= x.Mobile,
                    
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
                return await db.Customers.AnyAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower() && x.IsActive != false).ConfigureAwait(false);
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


        public async Task<DataTableResponseCarrier<CustomerViewModel>> GetLogList(DataTableFilterViewModel filterDto, int id)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.CustomerLogs.Where(x => x.CustomerId == id).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<CustomerViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new CustomerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address= x.Address,
                    Mobile= x.Mobile,
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
