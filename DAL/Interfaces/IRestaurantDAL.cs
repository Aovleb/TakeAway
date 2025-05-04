using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IRestaurantDAL
    {
        public Task<List<Restaurant>> GetRestaurantsForClientAsync(IServiceDAL serviceDAL);
        public Task<List<Restaurant>> GetRestaurantsForRestaurateurAsync(IServiceDAL serviceDAL, int restaurateurId);
        Task<Restaurant> GetRestaurantAsync(IServiceDAL serviceDAL, int id);
        public Task<bool> InsertRestaurantAsync(IServiceDAL serviceDAL, Restaurant restaurant, int userId);
    }
}
