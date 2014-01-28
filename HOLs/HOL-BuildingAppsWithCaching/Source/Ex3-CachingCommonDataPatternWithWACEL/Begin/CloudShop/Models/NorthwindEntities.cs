namespace CloudShop.Models
{
    using System.Data.Entity;

    public partial class NorthwindEntities : DbContext
    {
        public NorthwindEntities()
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}