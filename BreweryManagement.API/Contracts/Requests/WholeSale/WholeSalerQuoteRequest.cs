namespace BreweryManagement.API.Contracts.Requests.WholeSale
{
    public class WholeSalerQuoteRequest
    {
        public string WholeSalerId { get; set; }
        public List<BeerQuantity> QuoteDetails { get; set; }
    }

    public class BeerQuantity
    {
        public string BeerId { get; set; }
        public int Quantity { get; set; }
    }
}
