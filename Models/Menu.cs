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


        public Menu(int id, string name, string description, decimal price, Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService)
        {
            Dishes = new List<Dish>();
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


        public async override Task<bool> CreateAsync(IMealDAL mealDAL, int restaurantId)
        {
            return await mealDAL.CreateAsync(this, restaurantId);
        }
    }
}
