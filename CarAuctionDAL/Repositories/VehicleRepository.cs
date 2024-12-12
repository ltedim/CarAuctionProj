using CarAuctionCommon.Context;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionDAL.Repositories
{
    public class VehicleRepository(AuctionDbContext auctionDbContext) : IVehicleRepository
    {
        public async Task<List<Vehicle>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await auctionDbContext.Meds.ToListAsync(cancellationToken);
        }
    }
}
