using System.ComponentModel.DataAnnotations;

namespace BreweryManagement.API.Contracts.Requests.Beer
{
    public class AddBeerRequest
    {
        [Required]
        public string BeerName { get; set; }
        [Required]
        public decimal BeerAlcoholContent { get; set; }
        [Required]
        public decimal BeerPrice { get; set; }
        [Required]
        public string BrewerId { get; set; }
    }
}
