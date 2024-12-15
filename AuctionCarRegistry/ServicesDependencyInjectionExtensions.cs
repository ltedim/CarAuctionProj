using CarAuctionCommon.Interfaces;
using CarAuctionServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionRegistry
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
