using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IRestaurantDAL
    {
        public Task<List<Restaurant>> GetRestaurantsAsync(int restaurateurId = -1);
        Task<Restaurant> GetRestaurantAsync(int id, bool withMeals = false);
        public Task<bool> InsertRestaurantAsync(Restaurant restaurant, int userId);
    }
}
