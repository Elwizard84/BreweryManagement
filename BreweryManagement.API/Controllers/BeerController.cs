using BreweryManagement.API.Contracts.Requests.Beer;
using BreweryManagement.Domain.Models;
using BreweryManagement.Infrastructure.Exceptions;
using BreweryManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BreweryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeerController : ControllerBase
    {
        private readonly ILogger<BeerController> _logger;
        private readonly IBeerService _beerService;

        public BeerController(ILogger<BeerController> logger, IBeerService beerService)
        {
            _beerService = beerService;
            _logger = logger;
        }

        /// <summary>
        /// Get all the beers listed under the given Brewer
        /// </summary>
        /// <param name="brewerId">Unique GUID of the Brewer</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByBrewery")]
        [ProducesResponseType(typeof(IEnumerable<Beer>), 200)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult GetByBrewery([FromQuery] string brewerId)
        {
            try
            {
                return Ok(_beerService.GetByBrewer(brewerId));
                // .Include(b => b.Brewer)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return StatusCode(422, ex.Message);
            }
        }

        /// <summary>
        /// Add a new beer
        /// </summary>
        /// <param name="request">Beer details</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(Beer), 201)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult AddBeer([FromBody] AddBeerRequest request)
        {
            try
            {
                Beer beer = _beerService.AddBeer(new Beer()
                {
                    Name = request.BeerName,
                    AlcoholContent = request.BeerAlcoholContent,
                    Price = request.BeerPrice,
                    Brewer = new Brewer()
                    {
                        Id = request.BrewerId // This is dependent on Brewer for further Authorization and validation
                    }
                });

                return Created(beer.Id, beer);
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
        /// Remove an existing beer, this is dependent on Brewer for further Authorization and validation
        /// </summary>
        /// <param name="beerId">Unique GUID of the Beer</param>
        /// <param name="brewerId">Unique GUID of the Brewer</param>
        /// <returns></returns>
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult RemoveBeer([FromRoute] string beerId, [FromQuery] string brewerId)
        {
            try
            {
                _beerService.RemoveBeer(beerId, brewerId);

                return NoContent();
            }
            catch (ObjectNotFoundException nex)
            {
                _logger.LogInformation(nex.Message, new { beerId });
                return StatusCode(401, nex.Message);
            }
            catch (UnauthorizedException uex)
            {
                _logger.LogInformation(uex.Message, new { brewerId });
                return StatusCode(401, uex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, new { beerId, brewerId });
                return StatusCode(422, ex.Message);
            }
        }
    }
}
