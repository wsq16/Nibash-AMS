using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FlatManagement.ViewModel;

namespace FlatManagement.Models
{
    public class FlatDBContext:IdentityDbContext<ApplicationUser>
    {
        public FlatDBContext(DbContextOptions<FlatDBContext> options) : base(options)
        {
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //    {
        //        foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        //    }
        //}

        public DbSet<BillVM> Bills { get; set; }
        public DbSet<MaintenanceVM> Maintenances { get; set; }
        public DbSet<EnumModel> EnumValues { get; set; }
        public DbSet<EmployeeVM> Employees { get; set; }
        //Remove
        public DbSet<FlatVM> Flats { get; set; }
        public DbSet<TenantVM> Tenants { get; set; }
        //End Remove
        public DbSet<ServiceVM> Services { get; set; }
        public DbSet<OwnerVM> Owners { get; set; }
        public DbSet<ContractVM> Contracts { get; set; }
        public DbSet<CommitteeVM> Committees { get; set; }
        public DbSet<SupplierVM> Suppliers { get; set; }
        public DbSet<TransactionVM> Transactions { get; set; }
        public DbSet<CommonFundVM> CommonFunds { get; set; }
        public DbSet<FlatConfigVM> FlatConfigs { get; set; }
        public DbSet<ResolutionVM> Resolutions { get; set; }
        public DbSet<AgendaVM> Agendas { get; set; }
        public DbSet<FAQVM> Faqs { get; set; }
        public DbSet<DemoChkBoxClass> Category { get; set; }
        public DbSet<ApprovalLimitVM> ApprovalLimits { get; set; }
        public DbSet<ProcessVM> Processes { get; set; }
        public DbSet<DelegatePersonHistory> DelegatePersonHistorys { get; set; }
        public DbSet<CompanyVM> CompanyInfo { get; set; }

    }
}
