using Microsoft.EntityFrameworkCore;

namespace Week4Assigment.InMemoryCashe
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }


    }
}
