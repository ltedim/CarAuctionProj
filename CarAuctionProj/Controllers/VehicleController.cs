using CarAuctionCommon.Dtos;
using CarAuctionCommon.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionProj.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController(IVehicleService vehicleService) : Controller
    {
        [HttpGet]
        public async Task<List<VehicleDto>> Get(CancellationToken cancellationToken)
        {
            return await vehicleService.GetAllAsync(cancellationToken);
        }
    }
}
