using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
    }
}