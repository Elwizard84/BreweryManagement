using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using BreweryManagement.Infrastructure.Enums;
using BreweryManagement.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
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
        void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity, UpdateMode upMode);
        void SellToWholesaler(string wholeSalerId, string beerId, int quantity);
        dynamic GetQuote(dynamic request);
    }

    public class WholeSalerBeerService : IWholeSalerBeerService
    {
        private readonly IBeerService _beerService;
        private readonly IWholeSalerService _wholeSalerService;
        private readonly BaseRepository _wholeSalerBeerRepository;

        public WholeSalerBeerService(
            IWholeSalerService wholeSalerService,
            IBeerService beerService,
            BaseRepository wholeSalerBeerRepository)
        {
            _beerService = beerService;
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

        public void UpdateBeerStock(WholeSaler wholeSaler, Beer beer, int quantity, UpdateMode upMode)
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
                switch(upMode)
                {
                    case UpdateMode.Strict:
                        dbWsBeer.Quantity = quantity;
                        break;
                    case UpdateMode.Incremental:
                        dbWsBeer.Quantity += quantity;
                        break;
                }
                _wholeSalerBeerRepository.Update(dbWsBeer);
            }

            _wholeSalerBeerRepository.SaveChanges();
        }

        public void SellToWholesaler(string wholeSalerId, string beerId, int quantity)
        {
            UpdateBeerStock(new WholeSaler()
            {
                Id = wholeSalerId,
            }, new Beer()
            {
                Id = beerId,
            }, quantity, UpdateMode.Incremental);
        }

        public dynamic GetQuote(dynamic request)
        {
            string wholeSalerId = request.WholeSalerId as string;
            List<dynamic> beers = new List<dynamic>(request.QuoteDetails);

            // check if wholesaler exists
            var dbSaler = _wholeSalerService.GetWholeSaler(wholeSalerId);
            if (dbSaler == null)
                throw new ValidationException("The wholesaler must exist");

            List<string> beerIds = beers.Select(b => b.BeerId as string).ToList();
            List<WholeSalerBeer> stockBeers = _wholeSalerBeerRepository.WholeSalerBeers
                .Include(wsb => wsb.Beer)
                .Where(wsb => wsb.WholeSaler.Id == wholeSalerId && beerIds.Contains(wsb.Beer.Id)).ToList();

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
            decimal discount = 1;
            if (beers.Sum(b => b.Quantity) > 20)
                discount -= (decimal)0.2;
            else if (beers.Sum(b => b.Quantity) > 10)
                discount -= (decimal)0.1;

            return new
            {
                Beers = stockBeers.Select(sb => new { sb.Beer.Name, beers.Single(b => b.BeerId == sb.Beer.Id).Quantity, sb.Beer.Price }),
                Discount = (1 - discount) * 100,
                Total = stockBeers.Sum(sb => (decimal)beers.Single(b => b.BeerId == sb.Beer.Id).Quantity * sb.Beer.Price) * discount
            };
        }
    }
}
