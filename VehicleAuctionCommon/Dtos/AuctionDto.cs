using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;

namespace VehicleAuctionCommon.Dtos
{
    public record AuctionDto (int Id, int VehicleId, DateTime AuctionDateTime, AuctionStatus StatusId, int? WinBid, DateTime AuctionScheduledEndDateTime, DateTime? AuctionEndDateTime)
    {
        public Auction ToAuction()
        {
            return new Auction { Id = Id, VehicleId = VehicleId, AuctionDateTime = AuctionDateTime, StatusId = StatusId, WinBid = WinBid, AuctionScheduledEndDateTime = AuctionScheduledEndDateTime, AuctionEndDateTime = AuctionEndDateTime};
        }
    }
}
