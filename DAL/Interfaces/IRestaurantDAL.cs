using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IRestaurantDAL
    {
        Task<List<Restaurant>> GetRestaurantsAsync(IServiceDAL serviceDAL);
        Task<Restaurant> GetRestaurantAsync(int id);
    }
}
