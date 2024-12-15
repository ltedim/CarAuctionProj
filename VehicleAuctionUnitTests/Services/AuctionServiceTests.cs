using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Enums;
using VehicleAuctionCommon.Interfaces;
using VehicleAuctionCommon.Entities;
using VehicleAuctionServices.Services;
using Moq;

namespace VehicleAuctionUnitTests.Services
{
    public class AuctionServiceTests
    {
        private Mock<IAuctionRepository> auctionRepositoryMock;
        private Mock<IBidRepository> bidRepositoryMock;
        private AuctionService auctionService;

        [SetUp]
        public void Setup()
        {
            auctionRepositoryMock = new Mock<IAuctionRepository>();
            bidRepositoryMock = new Mock<IBidRepository>();
            auctionService = new AuctionService(auctionRepositoryMock.Object, bidRepositoryMock.Object);
        }

        [Test]
        public async Task CarIdNotValidExistsTest()
        {
            var auctionDto = new AuctionDto(0, 0, new DateTime(2024,12,23), AuctionStatus.NotStarted, null, new DateTime(2024, 12, 23, 23, 59, 59), null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.AddAsync(auctionDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Car Id must be valid."));
        }

        [Test]
        public async Task StartDateNotValidExistsTest()
        {
            var auctionDto = new AuctionDto(0, 1, new DateTime(2024, 12, 14), AuctionStatus.NotStarted, null, new DateTime(2024, 12, 23, 23, 59, 59), null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.AddAsync(auctionDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction DateTime has to be later than today."));
        }

        [Test]
        public async Task AuctionScheduledEndDateNotValidExistsTest()
        {
            var auctionDto = new AuctionDto(0, 1, new DateTime(2024, 12, 24), AuctionStatus.NotStarted, null, new DateTime(2024, 12, 23, 23, 59, 59), null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.AddAsync(auctionDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction Scheduled End DateTime has to be later than Auction DateTime."));
        }

        [Test]
        public async Task CarAlreadyInAnotherAuctionTest()
        {
            auctionRepositoryMock.Setup(a => a.IsVehicleAuctionActive(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(true);

            var auctionDto = new AuctionDto(0, 1, new DateTime(2024, 12, 16), AuctionStatus.NotStarted, null, new DateTime(2024, 12, 23, 23, 59, 59), null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.AddAsync(auctionDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction for this vehicle already exists."));
        }

        [Test]
        public async Task StartAuctionFailsDueAuctionDoesNotExistTest()
        {
            auctionRepositoryMock
                .Setup(a => a.FetchById(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync((Auction)null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.StartAsync(1, 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction Id is not valid."));
        }

        [Test]
        public async Task StartAuctionFailsDueWrongStatusTest()
        {
            auctionRepositoryMock.Setup(a => a.FetchById(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(
                new Auction {
                    Id = 1,
                    VehicleId = 1,
                    StatusId = AuctionStatus.Started,
                    AuctionScheduledEndDateTime = new DateTime(2024,12,20,23,59,59)
                }
            );

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.StartAsync(1, 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction no longer available to start."));
        }

        [Test]
        public async Task CloseAuctionFailsDueAuctionDoesNotExistTest()
        {
            auctionRepositoryMock
                .Setup(a => a.FetchById(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync((Auction)null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.CloseAsync(1, 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction Id is not valid."));
        }

        [Test]
        public async Task CloseAuctionFailsDueWrongStatusTest()
        {
            auctionRepositoryMock.Setup(a => a.FetchById(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(
                new Auction {
                    Id = 1,
                    VehicleId = 1,
                    StatusId = AuctionStatus.NotStarted,
                    AuctionScheduledEndDateTime = new DateTime(2024,12,20,23,59,59)
                }
            );

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService.CloseAsync(1, 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction no longer available to close."));
        }

        [Test]
        public async Task CloseAuctionSuccessTest()
        {
            var auctionToClose = new Auction {
                    Id = It.IsAny<int>(),
                    VehicleId = It.IsAny<int>(),
                    StatusId = AuctionStatus.Started,
                    AuctionScheduledEndDateTime = It.IsAny<DateTime>()
                };

            auctionRepositoryMock.Setup(a => a.FetchById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                value: auctionToClose
            );

            auctionRepositoryMock.Setup(a => a.UpdateAsync(auctionToClose, It.IsAny<CancellationToken>()));

            bidRepositoryMock.Setup(a => a.GetMaxForAuctionIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Bid {
                Id = 1,
                AuctionId = 1,
                AuctionBidDateTime = DateTime.UtcNow,
                BidValue = 305,
                BuyerId = 1
            });

            await auctionService.CloseAsync(1, CancellationToken.None);

            auctionRepositoryMock.Verify(x =>
                x.UpdateAsync(It.IsAny<Auction>(), It.IsAny<CancellationToken>()), Times.Exactly(2));

            bidRepositoryMock.Verify(x => 
                x.GetMaxForAuctionIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AddBidFailsDueAuctionDoesNotExistTest()
        {
            auctionRepositoryMock
                .Setup(a => a.FetchById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Auction)null);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService
            .AddBidAsync(new BidDto(1,1, DateTime.Now, 1003, 1), 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Invalid auction."));
        }

        [Test]
        public async Task AddBidFailsDueAuctionNotStartedTest()
        {
            var auctionToBid = new Auction {
                    Id = It.IsAny<int>(),
                    VehicleId = It.IsAny<int>(),
                    StatusId = AuctionStatus.NotStarted,
                    AuctionScheduledEndDateTime = It.IsAny<DateTime>()
                };

            auctionRepositoryMock
                .Setup(a => a.FetchById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionToBid);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await auctionService
            .AddBidAsync(new BidDto(1,1, DateTime.Now, 1003, 1), 
            CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Auction is not available for bids."));
        }

        [Test]
        public async Task AddBidWithSucessTest()
        {
            var auctionToBid = new Auction {
                    Id = It.IsAny<int>(),
                    VehicleId = It.IsAny<int>(),
                    StatusId = AuctionStatus.Started,
                    AuctionScheduledEndDateTime = It.IsAny<DateTime>()
                };

            auctionRepositoryMock
                .Setup(a => a.FetchById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionToBid);

            bidRepositoryMock
                .Setup(b => b.AddAsync(It.IsAny<Bid>(), It.IsAny<CancellationToken>()));

                var bidResult = await auctionService.AddBidAsync(new BidDto(1,1, DateTime.Now, 1003, 1), 
            CancellationToken.None);

            bidRepositoryMock.Verify(x => 
                x.AddAsync(It.IsAny<Bid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
