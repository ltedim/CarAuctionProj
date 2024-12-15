using VehicleAuctionCommon.Dtos;

namespace VehicleAuctionCommon.Entities
{
    public class Bid
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public DateTime AuctionBidDateTime { get; set; }
        public decimal BidValue { get; set; }
        public int BuyerId { get; set; }

        public BidDto ToBidDto()
        {
            return new BidDto(Id, AuctionId, AuctionBidDateTime, BidValue, BuyerId);
        }
    }
}
