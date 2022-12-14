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
    public class BaseRepository : DbContext
    {
        private readonly IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Default"));
        }

        public virtual DbSet<Beer> Beers { get; set; }
        public virtual DbSet<Brewer> Brewers { get; set; }
        public virtual DbSet<WholeSaler> WholeSalers { get; set; }
        public virtual DbSet<WholeSalerBeer> WholeSalerBeers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WholeSalerBeer>()
                .HasKey(c => c.Id);
        }
    }
}
