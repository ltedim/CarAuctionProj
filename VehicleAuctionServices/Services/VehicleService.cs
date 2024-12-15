using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Interfaces;

namespace VehicleAuctionServices.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<List<VehicleDto>> GetFilteredAsync(VehicleSearchDto? vehicleSearchDto, CancellationToken cancellationToken)
        {
            if(vehicleSearchDto == null)
            {
                var allVehivles = await vehicleRepository.GetAllAsync(cancellationToken);
                return allVehivles.Select(v => v.ToVehicleDto()).ToList();
            }

            var filteredVehicles = await vehicleRepository.GetFilteredAsync(vehicleSearchDto.TypeId, vehicleSearchDto.Manufacturer, vehicleSearchDto.Model, vehicleSearchDto.Year, cancellationToken);
            return filteredVehicles.Select(v => v.ToVehicleDto()).ToList();
        }

        public async Task<VehicleDto> AddAsync(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            if (vehicleDto.NumDoors < 1)
            {
                throw new ArgumentException("Number of doors has to be higher than zero.");
            }

            if (vehicleDto.Year < 1900 || vehicleDto.Year > DateTime.UtcNow.Year)
            {
                throw new ArgumentException("Year of the car should be higher than 1900.");
            }

            if (vehicleDto.StartingBid <= 0)
            {
                throw new ArgumentException("StartingBid should be higher than zero.");
            }

            var plateExists = await vehicleRepository.PlateExistsAsync(vehicleDto.Plate, cancellationToken);
            if (plateExists)
            {
                throw new ArgumentException("Vehicle already exists.");
            }

            var newVehicle = vehicleDto.ToVehicle();

            await vehicleRepository.AddAsync(newVehicle, cancellationToken);

            return newVehicle.ToVehicleDto();
        }
    }
}
