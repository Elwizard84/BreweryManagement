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
        public decimal AlcoholContent { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Brewer Brewer { get; set; }
    }
}
