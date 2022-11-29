﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VKTrackerEntities : DbContext
    {
        public VKTrackerEntities()
            : base("name=VKTrackerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Fabric> Fabrics { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<ParcelCode> ParcelCodes { get; set; }
        public virtual DbSet<StockCode> StockCodes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserOrganization> UserOrganizations { get; set; }
        public virtual DbSet<OrganizationLog> OrganizationLogs { get; set; }
        public virtual DbSet<FabricLog> FabricLogs { get; set; }
        public virtual DbSet<ItemLog> ItemLogs { get; set; }
        public virtual DbSet<LocationLog> LocationLogs { get; set; }
        public virtual DbSet<ParcelCodeLog> ParcelCodeLogs { get; set; }
        public virtual DbSet<StockCodeLog> StockCodeLogs { get; set; }
        public virtual DbSet<Userlog> Userlogs { get; set; }
        public virtual DbSet<ParcelReport> ParcelReports { get; set; }
        public virtual DbSet<ParcelReportLog> ParcelReportLogs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerLog> CustomerLogs { get; set; }
        public virtual DbSet<StockManagement> StockManagements { get; set; }
        public virtual DbSet<StockManagementLog> StockManagementLogs { get; set; }
    }
}
