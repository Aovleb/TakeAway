using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IDishDAL
    {
        Task<List<Dish>> GetDishesAsync(Restaurant r);
        Task<List<Dish>> GetDishesAsync(Menu m);
    }
}
