using CarAuctionCommon.Dtos;
using CarAuctionCommon.Enums;
using CarAuctionCommon.Interfaces;

namespace CarAuctionServices.Services
{
    public class AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository) : IAuctionService
    {
        public async Task<AuctionDto> AddAsync(AuctionDto auctionDto, CancellationToken cancellationToken)
        {
            if(auctionDto.CarId <= 0)
            {
                throw new ArgumentException("Car Id must be valid.");
            }

            if (auctionDto.AuctionDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Auction DateTime has to be later than today.");
            }

            if (auctionDto.AuctionScheduledEndDateTime <= auctionDto.AuctionDateTime)
            {
                throw new ArgumentException("Auction Scheduled End DateTime has to be later than Auction DateTime.");
            }

            var isActive = await auctionRepository.IsVehicleAuctionActive(auctionDto.CarId, cancellationToken);
            if (isActive)
            {
                throw new ArgumentException("Auction for this vehicle already exists.");
            }

            var newAuction = auctionDto.ToAuction();

            newAuction.AuctionEndDateTime = null;
            newAuction.StatusId = AuctionStatus.NotStarted;

            await auctionRepository.AddAsync(newAuction, cancellationToken);

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

            // 1st close auction
            auction.StatusId = AuctionStatus.Closed;
            auction.AuctionEndDateTime = DateTime.UtcNow;
            await auctionRepository.UpdateAsync(auction, cancellationToken);

            // 2nd update won bid, since auction is closed no new bid can be added
            var maxBid = await bidRepository.GetMaxForAuctionIdAsync(id, cancellationToken);
            if (maxBid != null)
            {
                auction.WinBid = maxBid.Id;
                await auctionRepository.UpdateAsync(auction, cancellationToken);
            }

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

            await auctionRepository.UpdateAsync(auction, cancellationToken);

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

            var newBid = bidDto.ToBid();
            newBid.AuctionBidDateTime = DateTime.UtcNow;

            await bidRepository.AddAsync(newBid, cancellation);

            return newBid.ToBidDto();
        }
    }
}
