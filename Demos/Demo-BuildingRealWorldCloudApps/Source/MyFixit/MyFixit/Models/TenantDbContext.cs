namespace MyFixIt.Models
{
    using System.Data.Entity;

    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<IssuingAuthorityKey> IssuingAuthorityKeys { get; set; }

        public DbSet<Tenant> Tenants { get; set; }
    }
}