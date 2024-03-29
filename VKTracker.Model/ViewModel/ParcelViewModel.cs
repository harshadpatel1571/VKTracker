﻿using System;
using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class ParcelViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Parcel is required.")]
        public int ParcelId { get; set; }

        public string Code { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public int LocationId { get; set; }

        public string LocationName { get; set; }

        [Required(ErrorMessage = "Challan No is required.")]
        public string ChallanNo { get; set; }
      
        public DateTime? DishpatchDate { get; set; }
        
        public DateTime? ArrivalDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [Required(ErrorMessage = "Challan Date is required.")]
        public DateTime? ChallanDate { get; set; }

        public string TransportNo { get; set; }
    }
}
