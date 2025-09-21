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
        private double price;
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
        [DataType(DataType.Currency)]
        public double Price
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


        public Meal(int id, string name, string description, double price, Service? lunchService, Service? dinnerService)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            LunchService = lunchService;
            DinnerService = dinnerService;
        }


        public Meal(int id, string name, string description, double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            LunchService = null;
            DinnerService = null;
        }


        public Meal()
        {
            LunchService = new Service();
            DinnerService = new Service();
        }


        public async static Task<Meal> GetMealAsync(IMealDAL mealDAL, int mealId)
        {
            return await mealDAL.GetMealAsync(mealId);
        }


        public abstract Task<bool> CreateAsync(IMealDAL mealDAL, int restaurantId);


        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Meal)
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
