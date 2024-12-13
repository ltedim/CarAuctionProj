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
            return await auctionDbContext.Vehicles.ToListAsync(cancellationToken);
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await auctionDbContext.Vehicles.AddAsync(vehicle, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            return vehicle;
        }
    }
}
