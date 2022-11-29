using System;

namespace VKTracker.Model.ViewModel
{
    public class StockManagementViewModel : BaseModel
    {
        public int Id { get; set; }
        public int ParcelId { get; set; }
        public string ParcelCode { get; set; }
        public int StockCodeId { get; set; }
        public string StockCode { get; set; }
        public int FabricId { get; set; }
        public string FabricName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal TotalQuantity { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
