namespace TakeAway.ViewModels
{
    public class BasketViewModel
    {
        public int? ClientId { get; set; }
        public Dictionary<int, int> Items { get; set; } = new Dictionary<int, int>(); // ID article : Quantité
        public decimal Total { get; set; }
        public string ServiceType { get; set; } // "Lunch" ou "Dinner"
        public int? RestaurantId { get; set; } // Bloque l'ajout de plats d'autres restaurants
        public string OrderType { get; set; } = "Pickup"; // "Delivery" ou "Pickup"
    }
}
