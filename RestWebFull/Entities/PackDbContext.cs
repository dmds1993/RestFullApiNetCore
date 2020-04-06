using Microsoft.EntityFrameworkCore;
using RestWebFull.Domain;

namespace RestWebFull.Entities
{
    public class PackDbContext : DbContext
    {
        public PackDbContext(DbContextOptions<PackDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
