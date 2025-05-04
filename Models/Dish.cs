using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Dish : Meal
    {
        public Dish(int id, string name, string description, decimal price, Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService) { }
        public Dish(int id, string name, string description, decimal price) : base(id, name, description, price, null,null) { }

        public Dish() { }

        public async static Task<List<Dish>> GetRestaurantDishesAsync(IDishDAL dishDAL, IServiceDAL serviceDAL,int id)
        {
            return await dishDAL.GetRestaurantDishesAsync(serviceDAL, id);
        }

        public async static Task<List<Dish>> GetDishesOfMenuAsync(IDishDAL dishDAL, int id)
        {
            return await dishDAL.GetDishesOfMenuAsync(id);
        }

        public async Task<bool> CreateAsync(IDishDAL dishDAL, IServiceDAL serviceDAL)
        {
            throw new NotImplementedException();
        }
    }
}
