using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;
using VehicleAuctionDAL.Repositories;
using VehicleAuctionUnitTests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionUnitTests.Repositories
{
    public class VehicleRepositoryTests
    {
        public VehicleRepository vehicleRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
            .UseInMemoryDatabase(databaseName: "auctiondbtest")
            .Options;

            // The dbContext faker is to help to inject data in the table to perform the tests
            var auctionDbContext = new AuctionDbContextFaker(options);
            vehicleRepository = new VehicleRepository(auctionDbContext);
        }

        [Test]
        public async Task AddVehicleTest()
        {
           
            var vehicle = new Vehicle
            {
                Id = 0,
                Plate = "123123FDFS",
                NumDoors = 4,
                LoadCapacity = 1000,
                Model = "Korpa",
                TypeId = VehicleType.Suv,
                Year = 2008,
                StartingBid = 2099,
                Manufacturer = "Seat"
            };


            await vehicleRepository.AddAsync(vehicle, CancellationToken.None);

            Assert.IsTrue(vehicle.Id == 5);
        }

        [Test]
        public async Task GetFilteredByManuBMWTest()
        {
            var result = await vehicleRepository.GetFilteredAsync(null, "BMW", null, null, CancellationToken.None);

            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public async Task GetFilteredByHatchBackTest()
        {
            var result = await vehicleRepository.GetFilteredAsync(VehicleType.HatchBack, null, null, null, CancellationToken.None);

            Assert.IsTrue(result.Count ==2);
        }
    }
}
