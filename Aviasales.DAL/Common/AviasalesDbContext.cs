using Aviasales.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aviasales.DAL.Common
{
    public class AviasalesDbContext : DbContext
    {
        public AviasalesDbContext()
        {
        }

        public AviasalesDbContext(DbContextOptions<AviasalesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AviasalesFlight> AviasalesFlights { get; set; }

        public virtual DbSet<AviasalesOrder> AviasalesOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}