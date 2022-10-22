using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IWholeSalerService
    {
        WholeSaler? GetSholeSaler(string id);
    }

    public class WholeSalerService : IWholeSalerService
    {
        private readonly WholeSalerRepository _wholeSalerRepository;

        public WholeSalerService(WholeSalerRepository wholeSalerRepository)
        {
            _wholeSalerRepository = wholeSalerRepository;
        }

        public WholeSaler? GetSholeSaler(string id)
        {
            return _wholeSalerRepository.WholeSalers.FirstOrDefault(x => x.Id == id);
        }
    }
}
