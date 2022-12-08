using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;
using System.Linq.Dynamic.Core;

namespace VKTracker.Repository.Repository
{
    public class DistributionRepository
    {
        /*public async Task<DataTableResponseCarrier<DistributionViewModel>> GetList(DataTableFilterViewModel filterDto, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var result = db.Distributions.Where(x => x.IsActive && x.UserId == userId && x.OrganizationId == organizationId).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.SearchValue))
                {
                    result = result.Where(x => x.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.StockCode.Code.Contains(filterDto.SearchValue) ||
                                                x.Location.LocationName.Contains(filterDto.SearchValue) ||
                                                x.ParcelCode.Code.Contains(filterDto.SearchValue) ||
                                                x.Fabric.FabricName.ToString().Contains(filterDto.SearchValue) ||
                                                x.ActualQuantity.ToString().Contains(filterDto.SearchValue) ||
                                                x.TotalQuantity.ToString().Contains(filterDto.SearchValue));
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
                    
                    LogUserName = db.Users.FirstOrDefault(u => u.Id == x.ModifiedBy).UserName,
                    CreatedOn = x.ModifiedOn,
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
        }*/
        public async Task<bool> SaveList(DistributionViewModel objModel, List<int> StockIds, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                var listModel = new List<Distribution>();
                foreach (var item in StockIds)
                {
                    var model = new Distribution();
                    model.CustomerId = objModel.PartyId;
                    model.Quantity = objModel.Quantity;
                    model.StockManagementId = item;
                    model.BillNo = objModel.BillNo;
                    model.DistributionDate = objModel.DistributionDate;
                    model.Note = objModel.Note;

                    listModel.Add(model);
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
