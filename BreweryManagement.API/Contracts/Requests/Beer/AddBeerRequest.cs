using System.ComponentModel.DataAnnotations;

namespace BreweryManagement.API.Contracts.Requests
{
    public class AddBeerRequest
    {
        [Required]
        public string BeerName { get; set; }
        [Required]
        public float BeerAlcoholContent { get; set; }
        [Required]
        public float BeerPrice { get; set; }
        [Required]
        public string BrewerId { get; set; }
    }
}
