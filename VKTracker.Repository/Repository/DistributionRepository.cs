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
