using CarAuctionCommon.Context;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionDAL.Repositories
{
    public class AuctionRepository(AuctionDbContext auctionDbContext) : IAuctionRepository
    {
        public async Task<Auction> AddAsync(Auction auction, CancellationToken cancellationToken)
        {
            // Transaction to lock o get do id e o save
            // Due to the use of InMemorary Db I want to replicate the auto increment

            var maxId = await auctionDbContext.Auctions.MaxAsync(c => c.Id);


            await auctionDbContext.Auctions.AddAsync(auction, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            return auction;
        }

        public async Task<Auction> UpdateAsync(Auction auction, CancellationToken cancellationToken)
        {
            auctionDbContext.Auctions.Update(auction);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            return auction;
        }

        public async Task<bool> IsCarAuctionActive(int carId, CancellationToken cancellationToken)
        {
            return await auctionDbContext.Auctions.Any(a => (a.StatusId == CarAuctionCommon.Enums.AuctionStatus.Started) && a.CarId == carId);
        }

        public async Task<int> GetMaxId(CancellationToken cancellationToken)
        {
            try
            {
                return await auctionDbContext.Auctions.MaxAsync(a => a.Id, cancellationToken);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<Auction?> FetchById(int id, CancellationToken cancellationToken)
        {
            return await auctionDbContext.Auctions.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}
