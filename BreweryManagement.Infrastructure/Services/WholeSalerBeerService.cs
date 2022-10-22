using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using BreweryManagement.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IWholeSalerBeerService
    {
        WholeSalerBeer? GetWholeSalerStock(WholeSaler wholeSaler);
        WholeSalerBeer? GetBeerStock(WholeSaler wholeSaler, Beer beer, out WholeSaler? dbSaler, out Beer? dbBeer);
        void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity);
        dynamic GetQuote(dynamic request);
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

        public WholeSalerBeer? GetWholeSalerStock(WholeSaler wholeSaler)
        {
            // check if wholesaler exists
            var dbSaler = _wholeSalerService.GetWholeSaler(wholeSaler.Id);
            if (dbSaler == null)
                throw new ObjectNotFoundException("Wholesaler was not found");
            string dbSalerId = dbSaler.Id;

            return _wholeSalerBeerRepository.WholeSalerBeers.FirstOrDefault(b => b.WholeSaler.Id == dbSalerId);
        }

        public WholeSalerBeer? GetBeerStock(WholeSaler wholeSaler, Beer beer, out WholeSaler? dbSaler, out Beer? dbBeer)
        {
            // check if wholesaler exists
            dbSaler = _wholeSalerService.GetWholeSaler(wholeSaler.Id);
            if (dbSaler == null)
                throw new ObjectNotFoundException("Wholesaler was not found");
            string dbSalerId = dbSaler.Id;

            // check if beer exists
            dbBeer = _beerService.GetById(beer.Id);
            if (dbBeer == null)
                throw new ObjectNotFoundException("Beer does not exist");
            string dbBeerId = dbBeer.Id;

            return _wholeSalerBeerRepository.WholeSalerBeers.FirstOrDefault(b =>
                                                                        b.WholeSaler.Id == dbSalerId &&
                                                                        b.Beer.Id == dbBeerId);
        }

        public void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity)
        {
            WholeSalerBeer? dbWsBeer = GetBeerStock(wholeSaler, beer, out WholeSaler? dbSaler, out Beer? dbBeer);

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

        public dynamic GetQuote(dynamic request)
        {
            string wholeSalerId = request.WholeSalerId as string;
            List<dynamic> beers = request.QuoteDetails as List<dynamic>;

            // check if wholesaler exists
            var dbSaler = _wholeSalerService.GetWholeSaler(wholeSalerId);
            if (dbSaler == null)
                throw new ValidationException("The wholesaler must exist");

            List<string> beerIds = beers.Select(b => b.Id as string).ToList();
            List<WholeSalerBeer> stockBeers = _wholeSalerBeerRepository.WholeSalerBeers.Where(wsb => wsb.WholeSaler.Id == wholeSalerId &&
                                                                   beerIds.Contains(wsb.Beer.Id)).ToList();

            // check if wholesaler sells these beer
            var stockBeerIds = stockBeers.Select(b => b.Beer.Id).ToList();
            var nonSoldBeers = beers.Where(b => !stockBeerIds.Contains(b.BeerId));
            if (nonSoldBeers.Any())
                throw new ValidationException($"The beer must be sold by the wholesaler: {string.Join(',', nonSoldBeers.Select(b => b.BeerId))}");

            //The number of beers ordered cannot be greater than the wholesaler's stock
            var lowStock = beers.Where(b => stockBeers.Single(sb => sb.Beer.Id == b.BeerId).Quantity < b.Quantity).ToList();
            if (lowStock.Any())
                throw new ValidationException($"The number of beers ordered cannot be greater than the wholesaler's stock: {string.Join(',', lowStock.Select(b => b.BeerId))}");

            // Discount
            float discount = 0;
            if (beers.Sum(b => b.Quantity) > 20)
                discount = 0.2f;
            if (beers.Sum(b => b.Quantity) > 20)
                discount = 0.1f;

            return new
            {
                Beers = stockBeers.Select(sb => new { sb.Beer.Name, beers.Single(b => b.BeerID == sb.Beer.Id).Quantity, sb.Beer.Price }),
                Discount = discount * 100,
                Total = stockBeers.Sum(sb => sb.Beer.Price * beers.Single(b => b.BeerID == sb.Beer.Id).Quantity) * discount
            };
        }
    }
}
