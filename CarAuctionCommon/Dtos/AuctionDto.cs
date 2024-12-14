using CarAuctionCommon.Entities;
using CarAuctionCommon.Enums;

namespace CarAuctionCommon.Dtos
{
    public record AuctionDto (int Id, int CarId, DateTime AuctionDateTime, AuctionStatus StatusId, int? WinningBid, DateTime AuctionEndScheduledDateTime, DateTime? AuctionEndDateTime)
    {
        public Auction ToAuction()
        {
            return new Auction { Id = Id, CarId = CarId, AuctionDateTime = AuctionDateTime, StatusId = StatusId, WinningBid = WinningBid, AuctionEndScheduledDateTime = AuctionEndScheduledDateTime, AuctionEndDateTime = AuctionEndDateTime};
        }
    }
}
