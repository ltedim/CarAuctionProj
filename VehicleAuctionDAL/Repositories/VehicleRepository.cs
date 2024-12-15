using VehicleAuctionCommon.Context;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;
using VehicleAuctionCommon.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VehicleAuctionDAL.Repositories
{
    public class VehicleRepository(AuctionDbContext auctionDbContext) : IVehicleRepository
    {
        public async Task<List<Vehicle>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await auctionDbContext.Vehicles.ToListAsync(cancellationToken);
        }

        public async Task<List<Vehicle>> GetFilteredAsync(VehicleType? typeId, string? manufacturer, string? model, int? year, CancellationToken cancellationToken)
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

        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            // This method should use a transaction but InMemories DBs does not allow (but stays as an example to a relational db as SqlServer or MySql
            // using var transaction = await auctionDbContext.Database.BeginTransactionAsync(cancellationToken);

            // This is to simulate the auto increment of the DB
            int id;
            try
            {
                id = await auctionDbContext.Vehicles.MaxAsync(a => a.Id, cancellationToken);
            }
            catch
            {
                id = 0;
            }
            vehicle.Id = id + 1;

            await auctionDbContext.Vehicles.AddAsync(vehicle, cancellationToken);
            await auctionDbContext.SaveChangesAsync(cancellationToken);

            // await transaction.CommitAsync(cancellationToken);
        }

        public async Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken)
        {
            return await auctionDbContext.Vehicles.AnyAsync(a => a.Plate == plate, cancellationToken);
        }
    }
}
