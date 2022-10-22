using BreweryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Repositories
{
    public class WholeSalerRepository : BaseRepository
    {
        public WholeSalerRepository(IConfiguration config) : base(config)
        {
        }

        public DbSet<WholeSaler> WholeSalers { get; set; }
    }
}
