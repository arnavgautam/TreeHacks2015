namespace Expenses.Web.Repository
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Expenses.Web.Models;

    public class ReportsContext : DbContext
    {
        public ReportsContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;            
        }

        public DbSet<ExpenseReport> ExpenseReports { get; set; }

        public DbSet<ExpenseReportDetail> ExpenseReportDetails { get; set; }

        public DbSet<ExpenseReportStatus> ExpenseReportStatuses { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
                
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();            
        }
    }
}