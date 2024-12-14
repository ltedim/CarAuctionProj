using CarAuctionCommon.Dtos;

namespace CarAuctionCommon.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetFilteredAsync(VehicleSearchDto? vehicleSearchDto, CancellationToken cancellationTokem);
        Task<VehicleDto> AddAsync(VehicleDto vehicleDto, CancellationToken cancellationToken);
    }
}
