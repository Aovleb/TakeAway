using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Menu : Meal
    {
        private List<Dish> dishes;
        public List<Dish> Dishes
        {
            get { return dishes; }
            set { dishes = value; }
        }
        public Menu(int id, string name, string description, decimal price,Service lunchService, Service dinnerService, List<Dish> dishes) : base(id, name, description, price, lunchService, dinnerService)
        {
            Dishes = dishes;
        }
        public Menu() : base()
        {
            Dishes = new List<Dish>();
        }

        public void AddDish(Dish dish)
        {
            if (dish != null && !Dishes.Contains(dish))
            {
                Dishes.Add(dish);
            }
        }

        public async static Task<List<Menu>> GetRestaurantMenusAsync(IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            return await menuDAL.GetRestaurantMenusAsync(dishDAL, serviceDAL, id);
        }

        public async Task<bool> CreateAsync(IMenuDAL menuDAL, int restaurantId)
        {
            return await menuDAL.CreateAsync(this, restaurantId);
        }
    }
}
