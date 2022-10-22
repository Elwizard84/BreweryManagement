using BreweryManagement.API.Contracts.Requests.WholeSale;
using BreweryManagement.API.Contracts.Requests.WholeSale.Validators;
using BreweryManagement.Domain.Models;
using BreweryManagement.Infrastructure.Exceptions;
using BreweryManagement.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WholeSalerController : ControllerBase
    {
        private readonly IWholeSalerBeerService _wholeSalerBeerService;
        private readonly ILogger<BeerController> _logger;

        public WholeSalerController(IWholeSalerBeerService wholeSalerBeerService, ILogger<BeerController> logger)
        {
            _wholeSalerBeerService = wholeSalerBeerService;
            _logger = logger;
        }

        /// <summary>
        /// Wholesaler update of beer quantity in stock
        /// </summary>
        /// <param name="request">Beer stock details</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateStock")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult UpdateBeerStock([FromBody] UpdateBeerStockRequest request)
        {
            try
            {
                _wholeSalerBeerService.UpdateBeerStock(new WholeSaler()
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

        /// <summary>
        /// Sell beer to wholesaler
        /// </summary>
        /// <param name="request">Sale details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SellToWholesaler")]
        [ProducesResponseType(typeof(Beer), 201)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult SellToWholesaler([FromBody] SellToWholeSalerRequest request)
        {
            try
            {
                _wholeSalerBeerService.SellToWholesaler(request.WholeSalerId, request.BeerId, request.Quantity);

                return Accepted();
            }
            catch (ObjectNotFoundException nex)
            {
                _logger.LogInformation(nex.Message, request);
                return StatusCode(401, nex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, request);
                return StatusCode(422, ex.Message);
            }
        }

        /// <summary>
        /// Quote a wholesaler for beers
        /// </summary>
        /// <param name="request">Quote details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetQuote")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult GetQuote([FromBody] WholeSalerQuoteRequest request)
        {
            try
            {
                WholeSalerQuoteRequestValidator validator = new WholeSalerQuoteRequestValidator();
                try
                {
                    validator.ValidateAndThrow(request);
                }
                catch (Exception iex)
                {
                    throw new ValidationException(iex.Message);
                }

                // Get quote
                return Ok(_wholeSalerBeerService.GetQuote(request));
            }
            catch (ValidationException vex)
            {
                _logger.LogInformation(vex.Message, request);
                return BadRequest(vex.Message);
            }
            catch (ObjectNotFoundException nex)
            {
                _logger.LogInformation(nex.Message, request);
                return StatusCode(401, nex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, request);
                return StatusCode(422, ex.Message);
            }
        }
    }
}
