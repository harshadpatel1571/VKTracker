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
    
    public partial class FabricLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public Nullable<int> FabricId { get; set; }
        public string FabricName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
    }
}
