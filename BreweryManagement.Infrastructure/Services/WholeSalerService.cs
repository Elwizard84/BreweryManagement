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
        WholeSaler? GetWholeSaler(string id);
    }

    public class WholeSalerService : IWholeSalerService
    {
        private readonly BaseRepository _wholeSalerRepository;

        public WholeSalerService(BaseRepository wholeSalerRepository)
        {
            _wholeSalerRepository = wholeSalerRepository;
        }

        public WholeSaler? GetWholeSaler(string id)
        {
            return _wholeSalerRepository.WholeSalers.FirstOrDefault(x => x.Id == id);
        }
    }
}
