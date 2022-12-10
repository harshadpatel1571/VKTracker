using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;
using System.Linq.Dynamic.Core;
using System.Data.Entity;
using System.Linq;

namespace VKTracker.Repository.Repository
{
    public class DistributionRepository
    {
        public async Task<DataTableResponseCarrier<DistributionViewModel>> GetList(DataTableFilterViewModel filterDto, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var result = db.Distributions.Where(x => x.IsActive && x.UserId == userId && x.OrganizationId == organizationId).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.StockManagement.Location.LocationName.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.StockManagement.Fabric.FabricName.Contains(filterDto.SearchValue)||
                                                x.StockManagement.Item.ItemName.Contains(filterDto.SearchValue)||
                                                x.StockManagement.TotalQuantity.ToString().Contains(filterDto.SearchValue)||
                                                x.StockManagement.ActualQuantity.ToString().Contains(filterDto.SearchValue));
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
                    Id= x.Id,
                    ParcelId = x.StockManagement.ParcelCode.Id,
                    ParcelCode = x.StockManagement.ParcelCode.Code,
                    FabricName= x.StockManagement.Fabric.FabricName,
                    ItemName = x.StockManagement.Item.ItemName,
                    TotalQuantity= x.StockManagement.TotalQuantity,
                    ActualQuantity= x.StockManagement.ActualQuantity,
                    LocationName = x.StockManagement.Location.LocationName                    
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
                    model.CreatedBy= userId;
                    model.CreatedOn = DateTime.Now;
                    model.UserId= userId;
                    model.OrganizationId = organizationId;

                    var modelStockCode = stockData.Where(x => x.Id == item).FirstOrDefault();
                    if (objModel.IsFull)
                    {
                        model.Quantity = modelStockCode.ActualQuantity;
                        modelStockCode.ActualQuantity = 0;
                    }
                    else
                    {
                        model.Quantity = objModel.Quantity;
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
    }
}
