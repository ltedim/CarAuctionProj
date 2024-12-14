using CarAuctionCommon.Context;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Enums;
using CarAuctionDAL.Repositories;
using CarAuctionUnitTests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionUnitTests.Repositories
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

            var auctionDbContext = new AuctionDbContextFaker(options);

            vehicleRepository = new VehicleRepository(auctionDbContext);

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
