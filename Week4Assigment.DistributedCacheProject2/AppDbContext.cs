using Microsoft.EntityFrameworkCore;

namespace Week4Assigment.DistributedCacheProject2
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }


    }
}
