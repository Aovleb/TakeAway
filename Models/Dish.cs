using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Dish : Meal
    {
        public Dish(int id, string name, string description, decimal price, Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService) { }

        public Dish() { }

        public async static Task<List<Dish>> GetDishesAsync(IDishDAL dishDAL, Restaurant r)
        {
            return await dishDAL.GetDishesAsync(r);
        }
    }
}
