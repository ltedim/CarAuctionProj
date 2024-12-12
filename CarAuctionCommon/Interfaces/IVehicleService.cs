using CarAuctionCommon.Dtos;
using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync(CancellationToken cancellationTokem);
    }
}
