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
    
    public partial class OrganizationLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Organization_Id { get; set; }
    }
}
