using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IMealDAL
    {
        Task<List<Meal>> GetMealsAsync(Restaurant r);
        Task<List<Meal>> GetMealsAsync(Order o);

    }
}
