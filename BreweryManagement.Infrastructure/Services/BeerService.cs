using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IBeerService
    {
        IEnumerable<Beer> GetByBrewery(string breweryId);
    }

    public class BeerService : IBeerService
    {

        private readonly BeerRepository _beerRepository;

        public BeerService(BeerRepository beerRepository)
        {
            _beerRepository = beerRepository;
        }

        public IEnumerable<Beer> GetByBrewery(string breweryId)
        {
            return _beerRepository.Beers.Where(b => b.Brewery.Id == breweryId);
        }
    }
}
