using System.ComponentModel.DataAnnotations;
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
        private Service? lunchService;
        private Service? dinnerService;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [Required(ErrorMessage = "Price is required.")]
        [Display(Name = "Price")]
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        [Display(Name = "Lunch Service")]
        public Service? LunchService
        {
            get { return lunchService; }
            set { lunchService = value; }
        }

        [Display(Name = "Dinner Service")]
        public Service? DinnerService
        {
            get { return dinnerService; }
            set { dinnerService = value; }
        }

        public Meal(int id, string name, string description, decimal price, Service? lunchService, Service? dinnerService)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            LunchService = lunchService;
            DinnerService = dinnerService;
        }

        public Meal()
        {
            LunchService = new Service();
            DinnerService = new Service();
        }

        public async static Task<List<Meal>> GetRestaurantMealsAsync(IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            return await mealDAL.GetRestaurantMealsAsync(menuDAL, dishDAL, serviceDAL, id);
        }

        public async static Task<Meal> GetMealAsync(IMealDAL mealDAL, IServiceDAL serviceDAL, IDishDAL dishDAL, int mealId)
        {
            return await mealDAL.GetMealAsync(serviceDAL, dishDAL, mealId);
        }

        public async Task<bool> AddInBasket(IMealDAL mealDAL, int clientId)
        {
            return await mealDAL.AddInBasket(this, clientId);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Meal other = (Meal)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
