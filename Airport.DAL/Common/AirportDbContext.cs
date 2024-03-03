using Airport.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Airport.DAL.Common
{
    public partial class AirportDbContext : DbContext
    {

        public AirportDbContext()
        {

        }

        public AirportDbContext(DbContextOptions<AirportDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }

}
