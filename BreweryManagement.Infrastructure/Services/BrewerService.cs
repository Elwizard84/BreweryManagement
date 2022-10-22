using BreweryManagement.Domain.Models;
using BreweryManagement.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Infrastructure.Services
{
    public interface IBrewerService
    {
        Brewer? GetById(string id);
    }

    public class BrewerService : IBrewerService
    {
        private readonly BaseRepository _brewerRepository;

        public BrewerService(BaseRepository brewerRepository)
        {
            _brewerRepository = brewerRepository;
        }

        public Brewer? GetById(string id)
        {
            return _brewerRepository.Brewers.FirstOrDefault(x => x.Id == id);
        }
    }
}
