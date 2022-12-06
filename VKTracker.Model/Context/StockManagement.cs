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
    
    public partial class StockManagement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockManagement()
        {
            this.Distributions = new HashSet<Distribution>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ParcelId { get; set; }
        public Nullable<int> StockCodeId { get; set; }
        public Nullable<int> FabricId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public decimal TotalQuantity { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public Nullable<int> UserId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual Fabric Fabric { get; set; }
        public virtual Item Item { get; set; }
        public virtual Location Location { get; set; }
        public virtual StockCode StockCode { get; set; }
        public virtual ParcelCode ParcelCode { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Distribution> Distributions { get; set; }
    }
}
