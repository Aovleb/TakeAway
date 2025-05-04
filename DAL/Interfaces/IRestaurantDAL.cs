using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IRestaurantDAL
    {
        public Task<List<Restaurant>> GetRestaurantsAsync(IServiceDAL serviceDAL, int restaurateurId = -1);
        Task<Restaurant> GetRestaurantAsync(IServiceDAL serviceDAL, IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL, int id);
        public Task<bool> InsertRestaurantAsync(IServiceDAL serviceDAL, Restaurant restaurant, int userId);
    }
}
