using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace VehicleAuctionDAL.Repositories
{
    public class BidRepository(AuctionDbContext auctionDbContext) : IBidRepository
    {
        public async Task AddAsync(Bid bid, CancellationToken cancellationToken)
        {
            // This method should use a transaction but InMemories DBs does not allow (but stays as an example to a relational db as SqlServer or MySql
            // using var transaction = await auctionDbContext.Database.BeginTransactionAsync(cancellationToken);

            var maxBid = await auctionDbContext.Bids.Where(b => b.AuctionId == bid.AuctionId).OrderByDescending(b => b.BidValue).FirstOrDefaultAsync(cancellationToken);

            if (maxBid != null && maxBid.BidValue > bid.BidValue)
            {
                throw new ArgumentException("Bid not valid.");
            }

            // This is to simulate the auto increment of the DB
            int id;
            try
            {
                id = await auctionDbContext.Bids.MaxAsync(a => a.Id, cancellationToken);
            }
            catch
            {
                id = 0;
            }
            bid.Id = id + 1;
            await auctionDbContext.Bids.AddAsync(bid, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            // await transaction.CommitAsync(cancellationToken);
        }

        public async Task<Bid?> GetMaxForAuctionIdAsync(int auctionID, CancellationToken cancellationToken)
        {
            return await auctionDbContext.Bids.Where(b => b.AuctionId == auctionID).OrderByDescending(b => b.BidValue).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
