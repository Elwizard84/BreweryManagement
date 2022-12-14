using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using BreweryManagement.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IBeerService
    {
        IEnumerable<Beer> GetByBrewer(string brewerId);
        Beer AddBeer(Beer beer);
        void RemoveBeer(string beerId, string brewerId);
        Beer? GetById(string id);
    }

    public class BeerService : IBeerService
    {
        private readonly BaseRepository _beerRepository;
        private readonly IBrewerService _brewerService;

        public BeerService(
            BaseRepository beerRepository, 
            IBrewerService brewerService)
        {
            _beerRepository = beerRepository;
            _brewerService = brewerService;
        }

        public Beer AddBeer(Beer beer)
        {
            // Check if brewer exists
            Brewer? brewer = _brewerService.GetById(beer.Brewer.Id);
            if (brewer == null)
                throw new ObjectNotFoundException("Could not find brewer for whom to add the beer");

            // update beer with fetched brewer
            beer.Brewer = brewer;

            // add beer
            _beerRepository.Add(beer);
            _beerRepository.SaveChanges();

            return beer;
        }

        public void RemoveBeer(string beerId, string brewerId)
        {
            // Validate beer exists
            Beer? beer = _beerRepository.Beers.Include(b => b.Brewer).FirstOrDefault(b => b.Id == beerId);
            if (beer == null)
                throw new ObjectNotFoundException("Requested beer was not found");

            // Validate that requesting brewer has access to delete beer
            if (beer.Brewer.Id != brewerId)
                throw new UnauthorizedException("Brewer not allowed to remove this beer");

            // Remove Beer
            _beerRepository.Remove(beer);
            _beerRepository.SaveChanges();
        }

        public IEnumerable<Beer> GetByBrewer(string brewerId)
        {
            return _beerRepository.Beers.Where(b => b.Brewer.Id == brewerId);
        }

        public Beer? GetById(string id)
        {
            return _beerRepository.Beers.FirstOrDefault(b => b.Id == id);
        }
    }
}
