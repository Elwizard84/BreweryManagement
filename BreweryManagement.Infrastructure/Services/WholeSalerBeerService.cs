﻿using BreweryManagement.Domain.Models;
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
        WholeSalerBeer? GetBeerStock(WholeSaler wholeSaler, Beer beer, out WholeSaler? dbSaler, out Beer? dbBeer);
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
    }
}
