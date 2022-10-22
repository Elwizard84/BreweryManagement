namespace BreweryManagement.API.Contracts.Requests.WholeSale
{
    public class UpdateBeerStockRequest
    {
        public string WholeSalerId { get; set; }
        public string BeerId { get; set; }
        public int NewQuantity { get; set; }
    }
}
