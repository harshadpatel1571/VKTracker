//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VKTracker.Model.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParcelReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParcelReport()
        {
            this.StockManagements = new HashSet<StockManagement>();
        }
    
        public int Id { get; set; }
        public int ParcelId { get; set; }
        public Nullable<System.DateTime> DispachedDate { get; set; }
        public string ChalanNo { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public int LocatoinId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public string TransportNo { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ParcelCode ParcelCode { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockManagement> StockManagements { get; set; }
    }
}
