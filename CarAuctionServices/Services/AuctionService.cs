using CarAuctionCommon.Dtos;
using CarAuctionCommon.Enums;
using CarAuctionCommon.Interfaces;

namespace CarAuctionServices.Services
{
    public class AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository) : IAuctionService
    {
        public async Task<AuctionDto> AddAsync(AuctionDto auctionDto, CancellationToken cancellationToken)
        {
            if(auctionDto.CarId > 0)
            {
                throw new ArgumentException("Car Id must be valid.");
            }

            if (auctionDto.AuctionDateTime > DateTime.Now)
            {
                throw new ArgumentException("Auction DateTime has to be later than today.");
            }

            if (auctionDto.AuctionScheduledEndDateTime > DateTime.Now && auctionDto.AuctionScheduledEndDateTime > auctionDto.AuctionDateTime)
            {
                throw new ArgumentException("Auction Scheduled End DateTime DateTime has to be later than today and later than Auction DateTime.");
            }

            var result = await auctionRepository.FetchActiveByCarId(auctionDto.CarId, cancellationToken);
            if (result.Count != 0)
            {
                throw new ArgumentException("Auction for this vehicle already exists.");
            }

            var newAuction = auctionDto.ToAuction();
            var lastId = await auctionRepository.GetMaxId(cancellationToken);

            newAuction.AuctionEndDateTime = null;
            newAuction.StatusId = AuctionStatus.NotStarted;

            if (result.Count > 0)
            {
                newAuction.Id = lastId + 1;
            }
            else
            {
                newAuction.Id = 1;
            }

            newAuction = await auctionRepository.AddAsync(newAuction, cancellationToken);

            return newAuction.ToAuctionDto();
        }

        public async Task<AuctionDto> CloseAsync(int id, CancellationToken cancellationToken)
        {
            var auction = await auctionRepository.FetchById(id, cancellationToken);

            if (auction == null)
            {
                throw new ArgumentException("Auction Id is not valid.");
            }
            
            if (auction.StatusId != AuctionStatus.Started)
            {
                throw new ArgumentException("Auction no longer available to close.");
            }

            // First close the auction in order to stop submiting more bids
            auction.StatusId = AuctionStatus.Closed;
            auction.AuctionEndDateTime = DateTime.UtcNow;
            auction = await auctionRepository.UpdateAsync(auction, cancellationToken);

            //Get the max bid and update auction
            var maxBid = await bidRepository.GetMaxForAuctionIdAsync(id, cancellationToken);
            if (maxBid != null)
            {
                //WonBid
                auction.WinningBid = maxBid.Id;
            }
            auction = await auctionRepository.UpdateAsync(auction, cancellationToken);

            return auction.ToAuctionDto();
        }

        public async Task<AuctionDto> StartAsync(int id, CancellationToken cancellationToken)
        {
            var auction = await auctionRepository.FetchById(id, cancellationToken);

            if (auction == null)
            {
                throw new ArgumentException("Auction Id is not valid.");
            }

            if (auction.StatusId != AuctionStatus.NotStarted)
            {
                throw new ArgumentException("Auction no longer available to start.");
            }

            auction.StatusId = AuctionStatus.Started;

            auction = await auctionRepository.UpdateAsync(auction, cancellationToken);

            return auction.ToAuctionDto();
        }

        public async Task<BidDto> AddBidAsync(BidDto bidDto, CancellationToken cancellation)
        {
            var auction = await auctionRepository.FetchById(bidDto.AuctionId, cancellation);

            if (auction == null)
            {
                throw new ArgumentException("Invalid auction.");
            } 
            else if(auction.StatusId != AuctionStatus.Started)
            {
                throw new ArgumentException("Auction is not available for bids.");
            }

            // Improvement: Move this to a queue system have a single consumer per auction in order to accept bids in a FIFO way
            // And prevent concurrency
            var maxBid = await bidRepository.GetMaxForAuctionIdAsync(auction.Id, cancellation);

            if(maxBid != null && maxBid.BidValue > bidDto.BidValue)
            {
                throw new ArgumentException("Bid not valid.");
            }

            var newBid = bidDto.ToBid();
            newBid.AuctionBidDateTime = DateTime.UtcNow;

            await bidRepository.AddAsync(newBid, cancellation);

            return newBid.ToBidDto();
        }
    }
}
