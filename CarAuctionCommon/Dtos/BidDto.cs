using CarAuctionCommon.Entities;

namespace CarAuctionCommon.Dtos
{
    public record BidDto(int Id, int AuctionId, DateTime AuctionBidDateTime, decimal BidValue, int BuyerId)
    {
        public Bid ToBid()
        {
            return new Bid { Id = Id, AuctionId = AuctionId, AuctionBidDateTime = AuctionBidDateTime, BidValue = BidValue, BuyerId = BuyerId };
        }
    }
}
