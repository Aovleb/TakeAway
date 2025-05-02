using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.DAL
{
    public class MealDAL : IMealDAL
    {
        private string connectionString;
        public MealDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Meal> GetMealAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Meal>> GetRestaurantMealsAsync(IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            List<Meal> meals = new List<Meal>();
            List<Menu> menus = await Menu.GetRestaurantMenusAsync(menuDAL, dishDAL, serviceDAL, id);
            meals.AddRange(menus);
            List<Dish> dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, id);
            meals.AddRange(dishes);
            return meals;
        }

        public async Task<List<Meal>> GetOrderMealsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
