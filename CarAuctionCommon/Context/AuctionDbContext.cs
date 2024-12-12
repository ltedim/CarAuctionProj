using CarAuctionCommon.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionCommon.Context
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var car = new List<Vehicle>
                {
                    new() { Id = 1, Plate = "123123FDFS", NumDoors = 4, LoadCapacity = 2000, Model = "Seat Ibiza", TypeId  = Enums.VehicleType.HatchBack, Year = 2008, StartingBid = 2099 },
                    new() { Id = 2, Plate = "555553AAFF", NumDoors = 5, LoadCapacity = 3000, Model = "Mercedes c200", TypeId  = Enums.VehicleType.Sedan, Year = 2010, StartingBid = 5199 }
                };

            modelBuilder.Entity<Vehicle>().HasData(car);
        }

        public DbSet<Vehicle> Vehicle { get; set; }
    }
}
