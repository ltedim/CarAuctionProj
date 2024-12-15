using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;
using VehicleAuctionDAL.Repositories;
using VehicleAuctionUnitTests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionUnitTests.Repositories
{
    public class AuctionRepositoryTests
    {
        private AuctionRepository auctionRepository;
        private AuctionDbContextFaker? auctionDbContextFaker;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
            .UseInMemoryDatabase(databaseName: "auctiondbtest")
            .Options;

            // The dbContext faker is to help to inject data in the table to perform the tests
            auctionDbContextFaker = new AuctionDbContextFaker(options);
            auctionRepository = new AuctionRepository(auctionDbContextFaker);
        }

        [Test]
        public async Task AddAuctionTest()
        {
           
            var auction = new Auction
            {
                Id = 0,
                VehicleId = 1,
                AuctionDateTime = DateTime.Now,
                StatusId = AuctionStatus.NotStarted,
                AuctionScheduledEndDateTime = DateTime.Now.AddDays(1)
            };

            await auctionRepository.AddAsync(auction, CancellationToken.None);

            Assert.IsTrue(auction.Id == 1);
        }

        [Test]
        public async Task VehicleIsActiveInAnotherAuctionTest()
        {
            await PrepareData();

            var result = await auctionRepository.IsVehicleAuctionActive(1, CancellationToken.None);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task VehicleAvailableForNewAuctionTest()
        {
            await PrepareData();

            var result = await auctionRepository.IsVehicleAuctionActive(5, CancellationToken.None);

            Assert.IsFalse(result);
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            auctionDbContextFaker.Dispose();
        }

        #region Helpers

        private async Task PrepareData()
        {
            var auction = new Auction
            {
                Id = 0,
                VehicleId = 1,
                AuctionDateTime = DateTime.Now,
                StatusId = AuctionStatus.NotStarted,
                AuctionScheduledEndDateTime = DateTime.Now.AddDays(1)
            };

            await auctionDbContextFaker.Auctions.AddAsync(auction, CancellationToken.None);

            var auction2 = new Auction
            {
                Id = 0,
                VehicleId = 2,
                AuctionDateTime = DateTime.Now,
                StatusId = AuctionStatus.NotStarted,
                AuctionScheduledEndDateTime = DateTime.Now.AddDays(1)
            };

            await auctionDbContextFaker.Auctions.AddAsync(auction2, CancellationToken.None);
        }

        #endregion
    }
}
