using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Enums;

namespace VehicleAuctionCommon.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime AuctionDateTime{ get; set; }
        public AuctionStatus StatusId { get; set; }
        public int? WinBid { get; set; }
        public DateTime AuctionScheduledEndDateTime { get; set; }
        public DateTime? AuctionEndDateTime { get; set; }

        public AuctionDto ToAuctionDto()
        {
            return new AuctionDto(Id, VehicleId, AuctionDateTime, StatusId, WinBid, AuctionScheduledEndDateTime, AuctionEndDateTime );
        }
    }
}
