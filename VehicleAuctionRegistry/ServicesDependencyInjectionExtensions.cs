using VehicleAuctionCommon.Interfaces;
using VehicleAuctionServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace VehicleAuctionRegistry
{
    public static class ServicesDependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IVehicleService, VehicleService>();
            serviceCollection.AddScoped<IAuctionService, AuctionService>();
            return serviceCollection;
        }
    }
}
