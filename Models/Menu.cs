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
        public Menu(int id, string name, string description, decimal price, List<Dish> dishes,Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService)
        {
            Dishes = dishes;
        }
        public Menu() : base()
        {
            Dishes = new List<Dish>();
        }

        public async static Task<List<Menu>> GetRestaurantMenusAsync(IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            return await menuDAL.GetRestaurantMenusAsync(dishDAL, serviceDAL, id);
        }
    }
}
