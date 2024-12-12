using CarAuctionCommon.Interfaces;
using CarAuctionDAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionCarRegistry
{
    public static class DALDependencyInjectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IVehicleRepository, VehicleRepository>();
            return serviceCollection;
        }
    }
}
