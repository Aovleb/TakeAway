using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IDishDAL
    {
        Task<List<Dish>> GetRestaurantDishesAsync(IServiceDAL serviceDAL, int id);
        Task<List<Dish>> GetDishesOfMenuAsync(int id);
    }
}
