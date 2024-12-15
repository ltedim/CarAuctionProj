using VehicleAuctionCommon.Dtos;
using VehicleAuctionCommon.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VehicleAuctionProj.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController(IVehicleService vehicleService) : Controller
    {
        [HttpGet]
        public async Task<List<VehicleDto>> GetFilteredAsync([FromQuery]VehicleSearchDto vehicleSearchDto, CancellationToken cancellationToken = default)
        {
            return await vehicleService.GetFilteredAsync(vehicleSearchDto, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Post(VehicleDto vehicleDto, CancellationToken cancellationToken = default)
        {
            try
            {
                return await vehicleService.AddAsync(vehicleDto, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
