namespace TakeAway.ViewModels
{
    public class BasketViewModel
    {
        public Dictionary<int, int> Items { get; set; } = new Dictionary<int, int>(); // ID article : Quantité
        public decimal Total { get; set; }
    }

    public class MealViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } // "Menu" ou "Dish"
        public List<MealViewModel> Dishes { get; set; } = new List<MealViewModel>(); // Liste des plats pour un menu
    }
}