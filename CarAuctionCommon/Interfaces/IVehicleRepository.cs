using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
        Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken);
    }
}
