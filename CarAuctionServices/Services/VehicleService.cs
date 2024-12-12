using CarAuctionCommon.Dtos;
using CarAuctionCommon.Interfaces;

namespace CarAuctionServices.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<List<VehicleDto>> GetAllAsync(CancellationToken cancellationTokem)
        {
            var result = await vehicleRepository.GetAllAsync(cancellationTokem);
            return result.Select(v => v.ToVehicleDto()).ToList();
        }
    }
}
