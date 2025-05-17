using TakeAway.Models;

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

    public class MealViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } // "Menu" ou "Dish"
        public Service LunchService { get; set; }
        public Service DinnerService { get; set; }
        public List<MealViewModel> Dishes { get; set; } = new List<MealViewModel>(); // Liste des plats pour un menu
        public int? RestaurantId { get; set; }
    }
}