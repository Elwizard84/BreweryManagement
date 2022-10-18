using BreweryManagement.Domain.Models;
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
        /// Gets all the beers listed under the given Brewery
        /// </summary>
        /// <param name="breweryId">Unique GUID of the Brewery</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByBrewery")]
        [ProducesResponseType(typeof(IEnumerable<Beer>), 200)]
        [ProducesResponseType(typeof(string), 422)]
        public ActionResult GetByBrewery([FromQuery] string breweryId)
        {
            try
            {
                return Ok(_beerService.GetByBrewery(breweryId));
                // .Include(b => b.Brewery)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return StatusCode(422, ex.Message);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //          _beerRepository.AddRange();
        //          _beerRepository.SaveChanges();
        //    }
        //    catch
        //    {
        //        return Ok();
        //    }
        //}
    }
}
