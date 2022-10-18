using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Models
{
    public class Beer : BaseModel
    {
        [Required]
        public float AlcoholContent { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public Brewery Brewery { get; set; }
    }
}
