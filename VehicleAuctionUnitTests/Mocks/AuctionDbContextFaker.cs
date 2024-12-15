using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionUnitTests.Mocks
{
    public class AuctionDbContextFaker : AuctionDbContext
    {
        public AuctionDbContextFaker(DbContextOptions<AuctionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var vehiclesList = GetAllVehiclesList();

            modelBuilder.Entity<Vehicle>().HasData(vehiclesList);
        }


        private List<Vehicle> GetAllVehiclesList()
        {
            return
            [
                new Vehicle { Id = 1, Plate = "d23eefwr2", NumDoors = 3, LoadCapacity = 3000, Model = "Corsa", TypeId = VehicleType.HatchBack,
                Year = 2005, StartingBid = 500, Manufacturer = "Opel" },
                new Vehicle { Id = 2, Plate = "123123FDFS", NumDoors = 3, LoadCapacity = 3000, Model = "Altiva", TypeId = VehicleType.HatchBack,
                Year = 2005, StartingBid = 350, Manufacturer = "Opel" },
                new Vehicle { Id = 3, Plate = "234sdffgdfg", NumDoors = 5, LoadCapacity = 3000, Model = "M4", TypeId = VehicleType.Sedan,
                Year = 2005, StartingBid = 9999, Manufacturer = "BMW" },
                new Vehicle { Id = 4, Plate = "TT234234T", NumDoors = 2, LoadCapacity = 50000, Model = "T200", TypeId = VehicleType.Truck,
                Year = 2005, StartingBid = 5000, Manufacturer = "Mercedes" },

            ];
        }
    }
}
