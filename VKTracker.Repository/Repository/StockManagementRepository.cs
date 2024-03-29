﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class StockManagementRepository
    {
        public async Task<DataTableResponseCarrier<StockManagementViewModel>> GetList(DataTableFilterViewModel filterDto, int userId, int organizationId, int? stockCodeId, int? fabricId, int? itemTypeId, int? availableQuantity, int? locationId, int? stockNo, DateTime? fromDate, DateTime? toDate)
        {
            var db = new VKTrackerEntities();
            try
            {
                var result = db.StockManagements.Where(x => (x.IsActive && x.OrganizationId == organizationId) &&
                                                    (stockCodeId.HasValue ? x.StockCodeId == stockCodeId : true) &&
                                                    (fabricId.HasValue ? x.FabricId == fabricId : true) &&
                                                    (itemTypeId.HasValue ? x.ItemId == itemTypeId : true) &&
                                                    (availableQuantity.HasValue ? (x.ActualQuantity > 0 && x.ActualQuantity >= availableQuantity) : true) &&
                                                    (locationId.HasValue ? x.LocationId == locationId : true) &&
                                                    (stockNo.HasValue ? x.Id == stockNo : true) &&
                                                    ((fromDate.HasValue ? DbFunctions.TruncateTime(x.ModifiedOn.Value) >= DbFunctions.TruncateTime(fromDate) : true) &&
                                                     (toDate.HasValue ? DbFunctions.TruncateTime(x.ModifiedOn.Value) <= DbFunctions.TruncateTime(toDate) : true))
                                                    ).AsNoTracking().AsQueryable();

                //.Where(x => x.IsActive && x.OrganizationId == organizationId).AsNoTracking().AsQueryable();
                //&& x.UserId == userId 
                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelReport.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.StockCode.Code.Contains(filterDto.SearchValue) ||
                                                x.Location.LocationName.Contains(filterDto.SearchValue) ||
                                                x.Fabric.FabricName.ToString().Contains(filterDto.SearchValue) ||
                                                x.ActualQuantity.ToString().Contains(filterDto.SearchValue) ||
                                                x.TotalQuantity.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<StockManagementViewModel>
                {
                    TotalCount = result.Count()
                };
                result = result.OrderByDescending(x => x.ModifiedOn);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new StockManagementViewModel
                {
                    Id = x.Id,
                    ParcelId = (int)x.ParcelId,
                    //ParcelCode = x.ParcelId.ToString() + "-" + DbFunctions.TruncateTime(x.ParcelReport.ArrivalDate).Value.ToString("yyy-MM-dd") + "-" + x.ParcelReport.ChalanNo,
                    ParcelCode = x.ParcelReport.ParcelCode.Code,
                    ArrivalDate = x.ParcelReport.ArrivalDate,
                    ChalanNo = x.ParcelReport.ChalanNo,
                    LocationId = (int)x.LocationId,
                    LocationName = x.Location.LocationName,
                    FabricId = (int)x.FabricId,
                    FabricName = x.Fabric.FabricName,
                    ItemId = (int)x.ItemId,
                    ItemName = x.Item.ItemName,
                    StockCodeId = (int)x.StockCodeId,
                    StockCode = x.StockCode.Code,
                    ActualQuantity = x.ActualQuantity,
                    TotalQuantity = x.TotalQuantity,
                    LogUserName = db.Users.FirstOrDefault(u => u.Id == x.ModifiedBy).UserName,
                    CreatedOn = x.ModifiedOn.Value
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

        public async Task<bool> Save(StockManagementViewModel objModel, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new StockManagement();

                if (objModel.Id > 0)
                {
                    model = await db.StockManagements.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.ParcelId = objModel.ParcelId;
                model.StockCodeId = objModel.StockCodeId;
                model.FabricId = objModel.FabricId;
                model.ItemId = objModel.ItemId;
                model.LocationId = objModel.LocationId;
                model.ActualQuantity = objModel.TotalQuantity;
                model.TotalQuantity = objModel.TotalQuantity;

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
                    model.ModifiedBy = objModel.CreatedBy;
                    model.ModifiedOn = DateTime.UtcNow;
                    db.StockManagements.Add(model);
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

        public async Task<StockManagementViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.StockManagements.Where(x => x.Id == id).Select(x => new StockManagementViewModel
                {
                    Id = x.Id,
                    ParcelId = (int)x.ParcelId,
                    StockCodeId = (int)x.StockCodeId,
                    FabricId = (int)x.FabricId,
                    ItemId = (int)x.ItemId,
                    LocationId = (int)x.LocationId,
                    ActualQuantity = x.ActualQuantity,
                    TotalQuantity = x.TotalQuantity,
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
                var model = await db.StockManagements.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<DataTableResponseCarrier<StockManagementViewModel>> GetLogList(DataTableFilterViewModel filterDto, int id)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.StockManagementLogs.Where(x => x.StockManagementId == id).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.StockManagementId.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<StockManagementViewModel>
                {
                    TotalCount = result.Count()
                };

                result = result.OrderBy(x => x.Id);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new StockManagementViewModel
                {
                    Id = x.Id,
                    ParcelId = (int)x.ParcelId,
                    ParcelCode = db.ParcelCodes.FirstOrDefault(c => c.Id == x.ParcelId).Code,
                    LocationId = (int)x.LocationId,
                    LocationName = db.Locations.FirstOrDefault(c => c.Id == x.LocationId).LocationName,
                    FabricId = (int)x.FabricId,
                    FabricName = db.Fabrics.FirstOrDefault(c => c.Id == x.FabricId).FabricName,
                    ItemId = (int)x.ItemId,
                    ItemName = db.Items.FirstOrDefault(c => c.Id == x.ItemId).ItemName,
                    StockCodeId = (int)x.StockCodeId,
                    StockCode = db.StockCodes.FirstOrDefault(c => c.Id == x.StockCodeId).Code,
                    TotalQuantity = (int)x.TotalQuantity,
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

        public async Task<bool> SaveList(List<StockManagementViewModel> objModel, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = objModel.Select(x => new StockManagement
                {
                    ParcelId = x.ParcelId,
                    StockCodeId = x.StockCodeId,
                    FabricId = x.FabricId,
                    ItemId = x.ItemId,
                    LocationId = x.LocationId,
                    ActualQuantity = x.TotalQuantity,
                    TotalQuantity = x.TotalQuantity,
                    IsActive = true,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.CreatedBy,
                    ModifiedOn = DateTime.UtcNow,
                    UserId = userId,
                    OrganizationId = organizationId > 0 ? organizationId : 0,
                }).ToList();

                db.StockManagements.AddRange(model);

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
        public async Task<bool> DeleteList(List<StockManagementViewModel> objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                foreach (var item in objModel)
                {
                    var model = await db.StockManagements.FirstOrDefaultAsync(x => x.Id == item.Id).ConfigureAwait(false);
                    model.IsActive = false;
                    model.CreatedBy = item.CreatedBy;
                    model.CreatedOn = item.CreatedOn;
                    db.Entry(model).State = EntityState.Modified;
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
        public async Task<bool> TransferLocation(List<StockManagementViewModel> objModel, int locationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                foreach (var item in objModel)
                {
                    var model = await db.StockManagements.FirstOrDefaultAsync(x => x.Id == item.Id).ConfigureAwait(false);
                    model.LocationId = locationId;
                    model.ModifiedBy = item.CreatedBy;
                    model.ModifiedOn = item.CreatedOn;
                    db.Entry(model).State = EntityState.Modified;
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
    }
}
