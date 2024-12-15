using VehicleAuctionCommon.Interfaces;
using VehicleAuctionDAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace VehicleAuctionRegistry
{
    public static class DALDependencyInjectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IVehicleRepository, VehicleRepository>();
            serviceCollection.AddScoped<IAuctionRepository, AuctionRepository>();
            serviceCollection.AddScoped<IBidRepository, BidRepository>();
            return serviceCollection;
        }
    }
}
