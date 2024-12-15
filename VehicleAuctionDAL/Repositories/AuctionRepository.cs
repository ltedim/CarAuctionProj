using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionDAL.Repositories
{
    public class AuctionRepository(AuctionDbContext auctionDbContext) : IAuctionRepository
    {
        public async Task AddAsync(Auction auction, CancellationToken cancellationToken)
        {
            // This method should use a transaction but InMemories DBs does not allow (but stays as an example to a relational db as SqlServer or MySql
            // using var transaction = await auctionDbContext.Database.BeginTransactionAsync(cancellationToken);

            // This is to simulate the auto increment of the DB
            int id;
            try
            {
                id = await auctionDbContext.Auctions.MaxAsync(a => a.Id, cancellationToken);
            }
            catch
            {
                id = 0;
            }

            auction.Id = id + 1;
            await auctionDbContext.Auctions.AddAsync(auction, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            // await transaction.CommitAsync(cancellationToken);
        }

        public async Task UpdateAsync(Auction auction, CancellationToken cancellationToken)
        {
            auctionDbContext.Auctions.Update(auction);
            await auctionDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsVehicleAuctionActive(int vehicleId, CancellationToken cancellationToken)
        {
            // Assuming that if an Auction is closed for a vehicle we are able to allow the creation opf a new auction for the same vehicle
            return await auctionDbContext.Auctions
                .AnyAsync(a => (a.StatusId != VehicleAuctionCommon.Enums.AuctionStatus.Closed) 
                    && a.VehicleId == vehicleId, cancellationToken);
        }

        public async Task<Auction?> FetchById(int id, CancellationToken cancellationToken)
        {
            return await auctionDbContext.Auctions.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}
