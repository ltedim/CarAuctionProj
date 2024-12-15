using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Entities;

namespace VehicleAuctionCommon.Interfaces
{
    public interface IBidRepository
    {
        Task<Bid?> GetMaxForAuctionIdAsync(int auctionID, CancellationToken cancellationToken);
        Task AddAsync(Bid bid, CancellationToken cancellationToken);
    }
}
