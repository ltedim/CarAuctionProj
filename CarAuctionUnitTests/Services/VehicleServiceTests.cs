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
        public async Task AddVehicleTest()
        {
            vehicleRepositoryMock.Setup(v => v.GetAllAsync(CancellationToken.None)).ReturnsAsync(new List<Vehicle>());
            var vehicle = new Vehicle{Id = It.IsAny<int>(), Plate = It.IsAny<string>(), NumDoors = It.IsAny<int>(), LoadCapacity = It.IsAny<int>(), 
            Model = It.IsAny<string>(), TypeId  = It.IsAny<VehicleType>(), Year = It.IsAny<int>(), StartingBid = It.IsAny<int>(), Manufacturer = It.IsAny<string>()};
            vehicleRepositoryMock.Setup(v => v.AddAsync(vehicle, CancellationToken.None)).ReturnsAsync(vehicle);

            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 4, 2000, "Ibiza", 2008, 2099, "Seat");

            var newVehicle = await vehicleService.AddAsync(vehicleDto, CancellationToken.None);

            Assert.IsTrue(newVehicle.VehicleId == 1);
        }

        [Test]
        public async Task AddSecondVehicleTest()
        {
            vehicleRepositoryMock.Setup(v => v.GetAllAsync(CancellationToken.None)).ReturnsAsync(GetVehicleList());
            var vehicle = new Vehicle{Id = It.IsAny<int>(), Plate = It.IsAny<string>(), NumDoors = It.IsAny<int>(), LoadCapacity = It.IsAny<int>(), 
            Model = It.IsAny<string>(), TypeId  = It.IsAny<VehicleType>(), Year = It.IsAny<int>(), StartingBid = It.IsAny<int>()};
            vehicleRepositoryMock.Setup(v => v.AddAsync(vehicle, CancellationToken.None)).ReturnsAsync(vehicle);

            var vehicleDto = new VehicleDto(0, "324234WEFWEF", VehicleType.HatchBack, 4, 2000, "Ibiza", 2008, 2099, "Seat");

            var newVehicle = await vehicleService.AddAsync(vehicleDto, CancellationToken.None);

            Assert.IsTrue(newVehicle.VehicleId == 3);
        }

        [Test]
        public async Task AddPlateExistsTest()
        {
            vehicleRepositoryMock.Setup(v => v.GetAllAsync(CancellationToken.None)).ReturnsAsync(GetVehicleList());
            var vehicle = new Vehicle{Id = It.IsAny<int>(), Plate = It.IsAny<string>(), NumDoors = It.IsAny<int>(), LoadCapacity = It.IsAny<int>(), 
            Model = It.IsAny<string>(), TypeId  = It.IsAny<VehicleType>(), Year = It.IsAny<int>(), StartingBid = It.IsAny<int>()};
            vehicleRepositoryMock.Setup(v => v.AddAsync(vehicle, CancellationToken.None)).ReturnsAsync(vehicle);

            var vehicleDto = new VehicleDto(0, "123123FDFS", VehicleType.HatchBack, 4, 2000, "Ibiza", 2008, 2099, "Seat");

            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await vehicleService.AddAsync(vehicleDto, CancellationToken.None));

            Assert.IsInstanceOf<ArgumentException>(exception);
            Assert.That(exception.Message, Is.EqualTo("Vehicle already exists."));
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
