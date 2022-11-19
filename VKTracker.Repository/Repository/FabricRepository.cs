﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Xml.Linq;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class FabricRepository
    {
        public async Task<DataTableResponseCarrier<FabricViewModel>> GetList(DataTableFilterDto filterDto)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.Fabrics.Where(x => x.IsActive).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.FabricName.Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<FabricViewModel>
                {
                    TotalCount = result.Count()
                };

                result = DynamicQueryableExtensions.OrderBy(result, filterDto.SortColumn + " " + filterDto.SortOrder);

                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                model.Data = await result.Select(x => new FabricViewModel
                {
                    Id = x.Id,
                    FabricName = x.FabricName
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

        public async Task<bool> Save(FabricViewModel objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new Fabric();

                if (objModel.Id > 0)
                {
                    model = await db.Fabrics.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.FabricName = objModel.FabricName;
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
                    db.Fabrics.Add(model);
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
                var model = await db.Fabrics.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<FabricViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Fabrics.Where(x => x.Id == id).Select(x => new FabricViewModel
                {
                    Id = x.Id,
                    FabricName = x.FabricName
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
                return await db.Fabrics.AnyAsync(x => x.Id != id && x.FabricName.ToLower() == name.ToLower()).ConfigureAwait(false);
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

        public async Task<List<BindDropdownViewModel>> BindOrganizationDDl()
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Fabrics.Where(x => x.IsActive).Select(x => new BindDropdownViewModel
                {
                    Id = x.Id,
                    Name = x.FabricName
                }).ToListAsync().ConfigureAwait(false);
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