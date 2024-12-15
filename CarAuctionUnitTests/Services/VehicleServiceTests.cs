using CarAuctionCommon.Interfaces;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Enums;
using CarAuctionCommon.Dtos;
using CarAuctionServices.Services;
using Moq;

namespace CarAuctionUnitTests.Services
{
    public class VehicleServiceTests
    {
        private Mock<IVehicleRepository> vehicleRepositoryMock;
        private VehicleService vehicleService;

        [SetUp]
        public void Setup()
        {
            vehicleRepositoryMock = new Mock<IVehicleRepository>();
            vehicleService = new VehicleService(vehicleRepositoryMock.Object);
        }

        [Test]
        public async Task PlateAlreadyExistsTest()
        {
            vehicleRepositoryMock.Setup(v => v.PlateExistsAsync(It.IsAny<string>(),  CancellationToken.None)).ReturnsAsync(true);

            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 4, 2000, "Ibiza", 2008, 2099, "Seat");

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await vehicleService.AddAsync(vehicleDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Vehicle already exists."));
        }

        [Test]
        public async Task WrongNumberOfDoorsTest()
        {
            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 0, 2000, "Ibiza", 2008, 2099, "Seat");

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await vehicleService.AddAsync(vehicleDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Number of doors has to be higher than zero."));
        }

        [Test]
        public async Task WrongYearTest()
        {
            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 3, 2000, "Ibiza", 2026, 2099, "Seat");

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await vehicleService.AddAsync(vehicleDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Year of the car should be higher than 1900."));
        }

        [Test]
        public async Task WrongStartingBidTest()
        {
            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 3, 2000, "Ibiza", 2020, 0, "Seat");

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await vehicleService.AddAsync(vehicleDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("StartingBid should be higher than zero."));
        }

        #region Helpers

        private List<Vehicle> GetVehicleList()
        {
            return
            [
                new Vehicle { Id = 1, Plate = "d23eefwr2", NumDoors = 3, LoadCapacity = 3000, Model = "Corsa", TypeId = VehicleType.HatchBack, 
                Year = 2005, StartingBid = 500, Manufacturer = "Open" },
                new Vehicle { Id = 2, Plate = "123123FDFS", NumDoors = 3, LoadCapacity = 3000, Model = "Altiva", TypeId = VehicleType.HatchBack, 
                Year = 2005, StartingBid = 500, Manufacturer = "Open" }
            ];
        }

        #endregion

    }
}
