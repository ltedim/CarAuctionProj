using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;
using VehicleAuctionDAL.Repositories;
using VehicleAuctionUnitTests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionUnitTests.Repositories
{
    public class BidRepositoryTests
    {
        private BidRepository bidRepository;
        private AuctionDbContextFaker? auctionDbContextFaker;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
            .UseInMemoryDatabase(databaseName: "auctiondbtest")
            .Options;

            // The dbContext faker is to help to inject data in the table to perform the tests
            auctionDbContextFaker = new AuctionDbContextFaker(options);
            bidRepository = new BidRepository(auctionDbContextFaker);
        }

        [Test]
        public async Task AddBidTest()
        {
            var newBid = new Bid
            {
                Id = 0,
                AuctionId = 1,
                AuctionBidDateTime = DateTime.Now,
                BidValue = 5000,
                BuyerId = 1
            };

            await bidRepository.AddAsync(newBid, CancellationToken.None);

            Assert.IsTrue(newBid.Id  > 0);
        }

        [Test]
        public async Task BidNotValidTest()
        {
            await PrepareData();

            var newBid = new Bid
            {
                Id = 0,
                AuctionId = 1,
                AuctionBidDateTime = DateTime.Now,
                BidValue = 1500,
                BuyerId = 1
            };

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => 
                await bidRepository.AddAsync(newBid, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Bid not valid."));
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            auctionDbContextFaker.Dispose();
        }

        #region Helpers

        private async Task PrepareData()
        {
            var newBid = new Bid
            {
                Id = 1,
                AuctionId = 1,
                AuctionBidDateTime = DateTime.Now,
                BidValue = 1505,
                BuyerId = 1
            };

            await auctionDbContextFaker.Bids.AddAsync(newBid, CancellationToken.None);

            var newBid2 = new Bid
            {
                Id = 2,
                AuctionId = 2,
                AuctionBidDateTime = DateTime.Now,
                BidValue = 2000,
                BuyerId = 5
            };

            await auctionDbContextFaker.Bids.AddAsync(newBid2, CancellationToken.None);
        }

        #endregion
    }
}
