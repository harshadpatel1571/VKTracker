using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;
using System.Linq.Dynamic.Core;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace VKTracker.Repository.Repository
{
    public class DistributionRepository
    {
        public async Task<DataTableResponseCarrier<DistributionViewModel>> GetList(DataTableFilterViewModel filterDto, int userId, int organizationId, int? stockCodeId, int? fabricId, int? itemTypeId, int? availableQuantity, int? locationId, string stockNo)
        {
            var db = new VKTrackerEntities();
            try
            {
                var result = db.Distributions.Where(x => (x.IsActive && x.UserId == userId && x.OrganizationId == organizationId) &&
                                                    (stockCodeId.HasValue ? x.StockManagement.StockCodeId == stockCodeId : true) &&
                                                    (fabricId.HasValue ? x.StockManagement.FabricId == fabricId : true) &&
                                                    (itemTypeId.HasValue ? x.StockManagement.ItemId == itemTypeId : true) &&
                                                    (availableQuantity.HasValue ? x.StockManagement.ActualQuantity == availableQuantity : true) &&
                                                    (locationId.HasValue ? x.StockManagement.LocationId == locationId : true) &&
                                                    (!string.IsNullOrEmpty(stockNo) ? x.BillNo == stockNo : true)
                                                    ).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.StockManagement.Location.LocationName.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.Fabric.FabricName.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.Item.ItemName.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.TotalQuantity.ToString().Contains(filterDto.SearchValue) ||
                                                x.StockManagement.ActualQuantity.ToString().Contains(filterDto.SearchValue) ||
                                                x.Customer.Name.ToString().Contains(filterDto.SearchValue) ||
                                                x.Customer.Address.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<DistributionViewModel>
                {
                    TotalCount = result.Count()
                };
                result = result.OrderBy(x => x.Id);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new DistributionViewModel
                {
                    Id = x.Id,
                    StockManagementId = (int)x.StockManagementId,
                    ParcelId = x.StockManagement.ParcelCode.Id,
                    StockCode = x.StockManagement.StockCode.Code,
                    FabricName = x.StockManagement.Fabric.FabricName,
                    ItemName = x.StockManagement.Item.ItemName,
                    TotalQuantity = x.StockManagement.TotalQuantity,
                    ActualQuantity = x.StockManagement.ActualQuantity,
                    LocationName = x.StockManagement.Location.LocationName,
                    CustomerName = x.Customer.Name,
                    CustomerAddress = x.Customer.Address,
                    DistributionDate = x.DistributionDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedOn
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
        public async Task<bool> SaveList(DistributionViewModel objModel, List<int> StockIds, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var listModel = new List<Distribution>();
                var stockData = db.StockManagements.Where(x => StockIds.Contains(x.Id)).ToList();

                foreach (var item in StockIds)
                {
                    var model = new Distribution();
                    model.CustomerId = objModel.PartyId;
                    model.StockManagementId = item;
                    model.BillNo = objModel.BillNo;
                    model.DistributionDate = objModel.DistributionDate;
                    model.Note = objModel.Note;
                    model.IsActive = true;
                    model.CreatedBy = userId;
                    model.CreatedOn = DateTime.Now;
                    model.UserId = userId;
                    model.OrganizationId = organizationId;

                    var modelStockCode = stockData.Where(x => x.Id == item).FirstOrDefault();
                    if (objModel.IsFull)
                    {
                        model.Quantity = modelStockCode.ActualQuantity;
                        modelStockCode.ActualQuantity = 0;
                        model.TypeId = objModel.IsFull;
                    }
                    else
                    {
                        model.Quantity = objModel.Quantity;
                        model.TypeId = objModel.IsFull;
                        modelStockCode.ActualQuantity = modelStockCode.ActualQuantity - objModel.Quantity;
                    }
                    modelStockCode.ModifiedBy = userId;
                    modelStockCode.ModifiedOn = DateTime.Now;

                    listModel.Add(model);
                    db.Entry(modelStockCode).State = EntityState.Modified;
                }
                db.Distributions.AddRange(listModel);
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
        public async Task<bool> DeleteList(List<DistributionViewModel> objModel)
        {
            var db = new VKTrackerEntities();
            try
            {
                foreach (var item in objModel)
                {
                    var model = await db.Distributions.FirstOrDefaultAsync(x => x.Id == item.Id).ConfigureAwait(false);
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

        public async Task<DataTableResponseCarrier<DistributionViewModel>> GetLogList(DataTableFilterViewModel filterDto, int id)
        {
            var db = new VKTrackerEntities();

            try
            {
                var result = db.DistributionLogs.Where(x => x.DistributionId == id).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.DistributionId.ToString().Contains(filterDto.SearchValue));
                }

                var model = new DataTableResponseCarrier<DistributionViewModel>
                {
                    TotalCount = result.Count()
                };

                result = result.OrderBy(x => x.Id);
                result = result.Skip(filterDto.Skip);

                if (filterDto.Take != -1)
                {
                    result = result.Take(filterDto.Take);
                }

                var response = result.Select(x => new DistributionViewModel
                {
                    Id = x.Id,
                    CustomerId = (int)x.CustomerId,
                    CustomerName = db.Customers.FirstOrDefault(c => c.Id == x.CustomerId).Name,
                    DistributionDate = x.DistributionDate,
                    StockManagementId = (int)x.StockManagementId,
                    StockCode = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).StockCode.Code, //  x.StockManagement.ParcelCode.Code,
                    FabricName = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).Fabric.FabricName,
                    ItemName = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).Item.ItemName,
                    TotalQuantity = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).TotalQuantity,
                    ActualQuantity = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).ActualQuantity,
                    LocationName = db.StockManagements.FirstOrDefault(c => c.Id == x.StockManagementId).Location.LocationName,
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

        public async Task<bool> Delete(int id, int userId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = await db.Distributions.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
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

        public async Task<bool> UpdateDistribution(DistributionViewModel objModel,int userId,int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var model = new Distribution();

                if (objModel.Id > 0)
                {
                    model = await db.Distributions.FirstOrDefaultAsync(x => x.Id == objModel.Id).ConfigureAwait(false);
                }

                model.CustomerId = objModel.PartyId;
                model.StockManagementId = objModel.StockCodeId;
                model.BillNo = objModel.BillNo;
                model.DistributionDate = objModel.DistributionDate;
                model.Note = objModel.Note;
                model.IsActive = true;
                model.CreatedBy = userId;
                model.CreatedOn = DateTime.Now;
                model.UserId = userId;
                model.OrganizationId = organizationId;

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
                    db.Distributions.Add(model);
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

        public async Task<DistributionViewModel> GetById(int id)
        {
            var db = new VKTrackerEntities();
            try
            {
                return await db.Distributions.Where(x => x.Id == id).Select(x => new DistributionViewModel
                {
                    Id = x.Id,
                    PartyId = (int)x.CustomerId,
                    DistributionDate= x.DistributionDate,
                    StockCodeId = (int)x.StockManagementId,
                    BillNo= x.BillNo,
                    IsFull= (bool)x.TypeId,
                    ActualQuantity = x.StockManagement.ActualQuantity,
                    Quantity = (int)x.Quantity,
                    Note= x.Note
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
