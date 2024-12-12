using CarAuctionCommon.Dtos;

namespace CarAuctionCommon.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync(CancellationToken cancellationTokem);
        Task<VehicleDto> AddAsync(VehicleDto vehicleDto, CancellationToken cancellationToken);
    }
}
