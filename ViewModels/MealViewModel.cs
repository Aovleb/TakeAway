using TakeAway.Models;

namespace TakeAway.ViewModels
{
    public class MealViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public Service LunchService { get; set; }
        public Service DinnerService { get; set; }
        public List<MealViewModel> Dishes { get; set; } = new List<MealViewModel>();
    }
}