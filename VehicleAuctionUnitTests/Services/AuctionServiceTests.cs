using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Enums;
using VehicleAuctionCommon.Interfaces;
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
    }
}
