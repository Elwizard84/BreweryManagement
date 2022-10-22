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
        private readonly IWholeSalerBeerService _woleSalerBeerService;
        private readonly ILogger<BeerController> _logger;

        public WholeSalerController(IWholeSalerBeerService woleSalerBeerService, ILogger<BeerController> logger)
        {
            _woleSalerBeerService = woleSalerBeerService;
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

        /// <summary>
        /// Quote a wholesaler for beers
        /// </summary>
        /// <param name="request">Quote details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetQuote")]
        [ValidateAntiForgeryToken]
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
                return Ok(_woleSalerBeerService.GetQuote(request));
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
