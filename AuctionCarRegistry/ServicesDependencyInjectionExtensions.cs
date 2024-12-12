using CarAuctionCommon.Interfaces;
using CarAuctionServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionCarRegistry
{
    public static class ServicesDependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IVehicleService, VehicleService>();
            return serviceCollection;
        }
    }
}
