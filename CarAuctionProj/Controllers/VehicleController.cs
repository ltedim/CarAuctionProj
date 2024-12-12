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
        public async Task<List<VehicleDto>> GetAll(CancellationToken cancellationToken)
        {
            return await vehicleService.GetAllAsync(cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Post(VehicleDto vehicleDto, CancellationToken cancellationToken)
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
