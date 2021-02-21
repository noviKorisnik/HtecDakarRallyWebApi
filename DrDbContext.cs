using Microsoft.EntityFrameworkCore;
using HtecDakarRallyWebApi.Models;

namespace HtecDakarRallyWebApi
{
    public class DrDbContext : DbContext
    {
        public DrDbContext(DbContextOptions<DrDbContext> options)
            : base(options)
        {
        }

        public DbSet<Race> Races { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleEvent> VehicleEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
//            builder.Entity<Race>().HasIndex(race => race.Year).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}