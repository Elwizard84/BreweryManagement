using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Models
{
    public class WholeSalerBeer
    {
        [Key]
        public string Id
        {
            get { return $"{WholeSaler?.Id}__{Beer?.Id}"; }
            set {  }
        }
        public WholeSaler WholeSaler { get; set; }
        public Beer Beer { get; set; }
        public int Quantity { get; set; }
    }
}
