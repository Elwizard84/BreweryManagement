using BreweryManagement.API.Contracts.Requests.WholeSale;
using BreweryManagement.Domain.Models;
using BreweryManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WholeSaleController : ControllerBase
    {
        private readonly IWholeSalerBeerService _woleSalerBeerService;
        private readonly ILogger<BeerController> _logger;

        public WholeSaleController(IWholeSalerBeerService woleSalerBeerService, ILogger<BeerController> logger)
        {
            _woleSalerBeerService = woleSalerBeerService;
            _logger = logger;
        }

        /// <summary>
        /// Wholesaler update of beer quantity in stock
        /// </summary>
        /// <param name="request">Beer stock details</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult UpdateBeerStock([FromBody] UpdateBeerStockRequest request)
        {
            try
            {
                _woleSalerBeerService.UpdateBeerStock(new WholeSaler()
                {
                    Id = request.WholeSalerId
                }, new Beer()
                {
                    Id = request.BeerId
                }, request.NewQuantity);

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return StatusCode(422, ex.Message);
            }
        }
    }
}
