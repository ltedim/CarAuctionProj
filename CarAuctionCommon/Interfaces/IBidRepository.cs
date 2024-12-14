using CarAuctionCommon.Dtos;
using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Interfaces
{
    public interface IBidRepository
    {
        Task<Bid?> GetMaxForAuctionIdAsync(int auctionID, CancellationToken cancellationToken);
        Task AddAsync(Bid bid, CancellationToken cancellationToken);
    }
}
