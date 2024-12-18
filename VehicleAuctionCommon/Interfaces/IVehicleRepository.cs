﻿using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Entities;
using VehicleAuctionCommon.Enums;

namespace VehicleAuctionCommon.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetFilteredAsync(VehicleType? typeId, string? manufacturer, string? model, int? year, CancellationToken cancellationToken);
        Task<List<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);
        Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken);
    }
}
