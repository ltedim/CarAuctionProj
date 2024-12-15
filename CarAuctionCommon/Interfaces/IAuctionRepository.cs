using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> AddAsync(Auction auction, CancellationToken cancellationToken);
        Task<Auction> UpdateAsync(Auction auction, CancellationToken cancellationToken);
        Task<bool> IsCarAuctionActive(int carId, CancellationToken cancellationToken);
        Task<int> GetMaxId(CancellationToken cancellationToken);
        Task<Auction?> FetchById(int id, CancellationToken cancellationToken);
    }
}
