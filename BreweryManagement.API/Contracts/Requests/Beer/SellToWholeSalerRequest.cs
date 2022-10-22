namespace BreweryManagement.API.Contracts.Requests.Beer
{
    public class SellToWholeSalerRequest
    {
        public string WholeSalerId { get; set; }
        public string BeerId { get; set; }
        public int Quantity { get; set; }
    }
}
