using TakeAway.Models;

namespace TakeAway.ViewModels
{
   public class MealViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } // "Menu" ou "Dish"
        public Service LunchService { get; set; }
        public Service DinnerService { get; set; }
        public List<MealViewModel> Dishes { get; set; } = new List<MealViewModel>(); // Liste des plats pour un menu
    }
}