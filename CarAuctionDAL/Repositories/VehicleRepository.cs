using CarAuctionCommon.Context;
using CarAuctionCommon.Entities;
using CarAuctionCommon.Enums;
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

        public async Task<List<Vehicle>> GetFilteredAsync(VehicleType? typeId, string manufacturer, string model, int? year, CancellationToken cancellationToken)
        {
            var result = auctionDbContext.Vehicles.AsQueryable();

            if (typeId != null)
            {
                result = result.Where(v => v.TypeId == typeId);
            }

            if (manufacturer != null)
            {
                result = result.Where(v => v.Manufacturer == manufacturer);
            }

            if (model != null)
            {
                result = result.Where(v => v.Model == model);
            }

            if (year != null)
            {
                result = result.Where(v => v.Year == year);
            }

            return await result.ToListAsync(cancellationToken);
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await auctionDbContext.Vehicles.AddAsync(vehicle, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            return vehicle;
        }
    }
}
