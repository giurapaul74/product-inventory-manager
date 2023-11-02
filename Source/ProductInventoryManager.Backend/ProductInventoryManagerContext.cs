global using Microsoft.EntityFrameworkCore;

namespace ProductInventoryManager.Backend
{
    public class ProductInventoryManagerContext : DbContext
    {
        public ProductInventoryManagerContext(DbContextOptions<ProductInventoryManagerContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=productInventoryManagerDb;Trusted_Connection=True;TrustServerCertificate=True");
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
