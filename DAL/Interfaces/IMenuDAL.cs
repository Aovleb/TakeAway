using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IMenuDAL
    {
        Task<List<Menu>> GetRestaurantMenusAsync(IDishDAL dishDAL, IServiceDAL serviceDAL, int id);
        Task<bool> CreateAsync(Menu menu, int restaurantId);
    }
}
