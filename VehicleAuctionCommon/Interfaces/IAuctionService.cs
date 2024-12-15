using VehicleAuctionCommon.Dtos;

namespace VehicleAuctionCommon.Interfaces
{
    public interface IAuctionService
    {
        Task<AuctionDto> AddAsync(AuctionDto auctionDto, CancellationToken cancellationToken);
        Task<AuctionDto> StartAsync(int id, CancellationToken cancellationToken);
        Task<AuctionDto> CloseAsync(int id, CancellationToken cancellationToken);
        Task<BidDto> AddBidAsync(BidDto bidDto, CancellationToken cancellation);

    }
}
