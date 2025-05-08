using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IMealDAL
    {
        Task<List<Meal>> GetRestaurantMealsAsync(IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id);
        Task<List<Meal>> GetOrderMealsAsync(int id);
        Task<Meal> GetMealAsync(IServiceDAL serviceDAL, IDishDAL dishDAL, int mealId);
        Task<bool> AddInBasket(Meal m, int clientId);

    }
}
