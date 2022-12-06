using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKTracker.Model.ViewModel
{
    public class DistributionViewModel
    {
        public int StockCodeId { get; set; }
        public int FabricId { get; set; }
        public int ItemTypeId { get; set; }
        public int AvailableQuantity { get; set; }
        public int LocationId { get; set; }
        public int StockNo { get; set; }
        public int PartyId { get; set; }
        public int Quantity { get; set; }
        public string BillNo { get; set; }
        public string Note { get; set; }
        public bool IsFull { get; set; }
        public DateTime? DistributionDate { get; set; }
    }

    //public class DistributionListModel
    //{
    //    public DistributionViewModel DistributionModel { get; set; }
    //    public List<int> StockIds { get; set; }
    //}
}
