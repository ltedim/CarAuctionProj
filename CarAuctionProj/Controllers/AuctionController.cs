using CarAuctionCommon.Dtos;
using CarAuctionCommon.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionProj.Controllers
{
    [Route("api/auction")]
    [ApiController]
    public class AuctionController(IAuctionService auctionService) : Controller
    {
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> Post(AuctionDto auctionDto, CancellationToken cancellationToken)
        {
            try
            {
                return await auctionService.AddAsync(auctionDto, cancellationToken);
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

        [Route("close")]
        [HttpPut]
        public async Task<ActionResult<AuctionDto>> Close(int auctionId, CancellationToken cancellationToken)
        {
            try
            {
                return await auctionService.CloseAsync(auctionId, cancellationToken);
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

        [Route("start")]
        [HttpPut]
        public async Task<ActionResult<AuctionDto>> Start(int auctionId, CancellationToken cancellationToken)
        {
            try
            {
                return await auctionService.StartAsync(auctionId, cancellationToken);
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

        [Route("place-bid")]
        [HttpPut]
        public async Task<ActionResult<BidDto>> PlaceBid(BidDto bidDto, CancellationToken cancellationToken)
        {
            try
            {
                return await auctionService.AddBidAsync(bidDto, cancellationToken);
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
