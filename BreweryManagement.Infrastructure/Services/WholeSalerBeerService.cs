using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using BreweryManagement.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IWholeSalerBeerService
    {
        void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity);
    }

    public class WholeSalerBeerService : IWholeSalerBeerService
    {
        private readonly IBeerService _beerService;
        private readonly IWholeSalerService _wholeSalerService;
        private readonly WholeSalerBeerRepository _wholeSalerBeerRepository;

        public WholeSalerBeerService(IWholeSalerService wholeSalerService, WholeSalerBeerRepository wholeSalerBeerRepository)
        {
            _wholeSalerService = wholeSalerService;
            _wholeSalerBeerRepository = wholeSalerBeerRepository;
        }

        public void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity)
        {
            // check if wholesaler exists
            WholeSaler? dbSaler = _wholeSalerService.GetSholeSaler(wholeSaler.Id);
            if (dbSaler == null)
                throw new UnauthorizedException("Wholesaler was not found");

            // check if beer exists
            Beer? dbBeer = _beerService.GetById(beer.Id);
            if (dbBeer == null)
                throw new Exception("Beer does not exist");

            // Update or Add stock
            WholeSalerBeer? dbWsBeer = _wholeSalerBeerRepository.WholeSalerBeers.FirstOrDefault(b =>
                                                                        b.WholeSaler.Id == dbSaler.Id &&
                                                                        b.Beer.Id == dbBeer.Id);

            if (dbWsBeer == null)
            {
                WholeSalerBeer newWsBeer = new WholeSalerBeer()
                {
                    WholeSaler = dbSaler,
                    Beer = dbBeer,
                    Quantity = quantity
                };
                _wholeSalerBeerRepository.Add(newWsBeer);
            } else
            {
                dbWsBeer.Quantity = quantity;
                _wholeSalerBeerRepository.Update(dbWsBeer);
            }

            _wholeSalerBeerRepository.SaveChanges();
        }
    }
}
