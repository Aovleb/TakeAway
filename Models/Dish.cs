using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Dish : Meal
    {
        public Dish(int id, string name, string description, decimal price, Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService) { }
        public Dish(int id, string name, string description, decimal price) : base(id, name, description, price) { }

        public Dish() : base() { }

        public async override Task<bool> CreateAsync(IMealDAL mealDAL, int restaurantId)
        {
            return await mealDAL.CreateAsync(this, restaurantId);
        }
    }
}
