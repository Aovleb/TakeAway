namespace TakeAway.ViewModels
{
    public class BasketViewModel
    {
        public int? ClientId { get; set; }
        public Dictionary<int, int> Items { get; set; } = new Dictionary<int, int>();
        public decimal Total { get; set; }
        public string ServiceType { get; set; }
        public int? RestaurantId { get; set; }
        public string OrderType { get; set; } = "Pickup";
    }
}
