using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Interfaces
{
    public interface IAuctionRepository
    {
        Task AddAsync(Auction auction, CancellationToken cancellationToken);
        Task UpdateAsync(Auction auction, CancellationToken cancellationToken);
        Task<bool> IsVehicleAuctionActive(int carId, CancellationToken cancellationToken);
        Task<Auction?> FetchById(int id, CancellationToken cancellationToken);
    }
}
