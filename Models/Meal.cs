using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public abstract class Meal
    {
        private int id;
        private string name;
        private string description;
        private decimal price;
        private Service lunchService;
        private Service dinnerService;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public Service LunchService
        {
            get { return lunchService; }
            set { lunchService = value; }
        }
        public Service DinnerService
        {
            get { return dinnerService; }
            set { dinnerService = value; }
        }

        public Meal(int id, string name, string description, decimal price, Service lunchService, Service dinnerService)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            LunchService = lunchService;
            DinnerService = dinnerService;
        }
        public Meal() { }
        public async static Task<List<Meal>> GetRestaurantMealsAsync(IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            return await mealDAL.GetRestaurantMealsAsync(menuDAL, dishDAL, serviceDAL, id);
        }
    }
}
