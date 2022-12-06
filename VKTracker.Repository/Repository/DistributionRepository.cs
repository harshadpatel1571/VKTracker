using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository.Repository
{
    public class DistributionRepository
    {
        public async Task<bool> SaveList(DistributionViewModel objModel, List<int> StockIds, int userId, int organizationId)
        {
            var db = new VKTrackerEntities();
            try
            {
                //var model = objModel.Select(x => new StockManagement
                //{
                //    ParcelId = x.ParcelId,
                //    StockCodeId = x.StockCodeId,
                //    FabricId = x.FabricId,
                //    ItemId = x.ItemId,
                //    LocationId = x.LocationId,
                //    TotalQuantity = x.TotalQuantity,
                //    IsActive = true,
                //    CreatedBy = x.CreatedBy,
                //    CreatedOn = x.CreatedOn,
                //    UserId = x.UserId,
                //    OrganizationId = x.OrganizationId
                //}).ToList();

                //db.StockManagements.AddRange(model);

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
