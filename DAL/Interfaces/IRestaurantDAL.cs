using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IRestaurantDAL
    {
        public Task<List<Restaurant>> GetRestaurantsAsync(IServiceDAL serviceDAL, int restaurateurId = -1);
        Task<Restaurant> GetRestaurantAsync(int id, IServiceDAL serviceDAL, IMealDAL mealDAL = null, IMenuDAL menuDAL = null, IDishDAL dishDAL = null, bool withMeals = false);
        public Task<bool> InsertRestaurantAsync(IServiceDAL serviceDAL, Restaurant restaurant, int userId);
    }
}
