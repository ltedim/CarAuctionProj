using CarAuctionCommon.Context;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionDAL.Repositories
{
    public class BidRepository(AuctionDbContext auctionDbContext) : IBidRepository
    {
        public async Task AddAsync(Bid bid, CancellationToken cancellationToken)
        {
            var maxId = await auctionDbContext.Bids.Where(b => b.AuctionId == bid.AuctionId).MaxAsync(b => b.Id);
            bid.Id = maxId + 1;

            await auctionDbContext.Bids.AddAsync(bid, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Bid?> GetMaxForAuctionIdAsync(int auctionID, CancellationToken cancellationToken)
        {
            var bids = await auctionDbContext.Bids.Where(b => b.AuctionId == auctionID).ToListAsync(cancellationToken);

            if(bids == null || bids.Count == 0)
            {
                return null;
            }

            return bids.OrderByDescending(b => b.BidValue).FirstOrDefault();
        }
    }
}
