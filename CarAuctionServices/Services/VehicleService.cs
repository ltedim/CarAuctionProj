using CarAuctionCommon.Dtos;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Interfaces;
using System.Threading;

namespace CarAuctionServices.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<List<VehicleDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await vehicleRepository.GetAllAsync(cancellationToken);
            return result.Select(v => v.ToVehicleDto()).ToList();
        }

        public async Task<VehicleDto> AddAsync(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            if(vehicleDto.NumDoors < 1)
            {
                throw new ArgumentException("Number of doors has to be higher than zero.");
            }

            if (vehicleDto.Year < 1900 && vehicleDto.Year > DateTime.UtcNow.Year)
            {
                throw new ArgumentException("Year of the car should be higher than 1900.");
            }

            if (vehicleDto.StartingBid > 0)
            {
                throw new ArgumentException("StartingBid should be higher than zero.");
            }

            var result = await vehicleRepository.GetAllAsync(cancellationToken);
            if(result.Any(v => v.Plate == vehicleDto.Plate))
            {
                throw new ArgumentException("Vehicle already exists.");
            }

            var newVehicle = vehicleDto.ToVehicle();
            if (result.Count > 0)
            {
                newVehicle.Id = result.Max(v => v.Id) + 1;
            }
            else
            {
                newVehicle.Id = 1;
            }

            newVehicle = await vehicleRepository.AddAsync(newVehicle, cancellationToken);

            return newVehicle.ToVehicleDto();
        }
    }
}
