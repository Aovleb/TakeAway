using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IMealDAL
    {
        Task<Meal> GetMealAsync(int mealId);
        Task<bool> CreateAsync(Dish dish, int restaurantId);
        Task<bool> CreateAsync(Menu menu, int restaurantId);

    }
}
