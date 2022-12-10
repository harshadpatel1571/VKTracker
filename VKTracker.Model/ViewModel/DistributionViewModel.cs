using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKTracker.Model.ViewModel
{
    public class DistributionViewModel : BaseModel
    {
        public int Id { get; set; }
        public int ParcelId { get; set; }
        public string ParcelCode { get; set; }
        public int StockCodeId { get; set; }
        public string StockCode { get; set; }
        public int FabricId { get; set; }
        public string FabricName { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemName { get; set; }
        public int AvailableQuantity { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int StockNo { get; set; }
        public int PartyId { get; set; }
        public int Quantity { get; set; }
        public string BillNo { get; set; }
        public string Note { get; set; }
        public bool IsFull { get; set; }
        public DateTime? DistributionDate { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
    }
}
