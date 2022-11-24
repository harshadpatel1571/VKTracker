using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKTracker.Model.ViewModel
{
    public class ParcelViewModel : BaseModel
    {
        public int Id { get; set; }

        public int ParcelId { get; set; }

        public int LocationId { get; set; }

        public string ChallanNo { get; set; }

        public DateTime DishpatchDate { get; set; }

        public DateTime ArrivalDate { get; set; }
    }
}
