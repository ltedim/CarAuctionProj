using CarAuctionCommon.Dtos;
using CarAuctionCommon.Interfaces;

namespace CarAuctionServices.Services
{
    public class AuctionService(IAuctionRepository auctionRepository) : IAuctionService
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

            if (auctionDto.AuctionEndScheduledDateTime > DateTime.Now && auctionDto.AuctionEndScheduledDateTime > auctionDto.AuctionDateTime)
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
            newAuction.StatusId = CarAuctionCommon.Enums.AuctionStatus.NotStarted;

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
                throw new InvalidOperationException("Auction Id is not valid.");
            }
            
            if (auction.StatusId != CarAuctionCommon.Enums.AuctionStatus.Started)
            {
                throw new InvalidOperationException("Auction no longer available to close.");
            }

            auction.StatusId = CarAuctionCommon.Enums.AuctionStatus.Closed;
            auction.AuctionEndDateTime = DateTime.UtcNow;

            auction = await auctionRepository.UpdateAsync(auction, cancellationToken);

            return auction.ToAuctionDto();
        }

        public async Task<AuctionDto> StartAsync(int id, CancellationToken cancellationToken)
        {
            var auction = await auctionRepository.FetchById(id, cancellationToken);

            if (auction == null)
            {
                throw new InvalidOperationException("Auction Id is not valid.");
            }

            if(auction.StatusId != CarAuctionCommon.Enums.AuctionStatus.NotStarted)
            {
                throw new InvalidOperationException("Auction no longer available to start.");
            }

            auction.StatusId = CarAuctionCommon.Enums.AuctionStatus.Started;

            auction = await auctionRepository.UpdateAsync(auction, cancellationToken);

            return auction.ToAuctionDto();
        }
    }
}
