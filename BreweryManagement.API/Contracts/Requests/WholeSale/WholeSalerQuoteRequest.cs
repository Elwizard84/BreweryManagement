namespace BreweryManagement.API.Contracts.Requests.WholeSale
{
    public class WholeSalerQuoteRequest
    {
        public string WholeSalerId { get; set; }
        public List<BeerQuantiy> QuoteDetails { get; set; }
    }

    public class BeerQuantiy
    {
        public string BeerId { get; set; }
        public int Quantity { get; set; }
    }
}
