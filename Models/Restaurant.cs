using System.ComponentModel.DataAnnotations;
using TakeAway.DAL.Interfaces;
using TakeAway.Validations;

namespace TakeAway.Models
{
    public class Restaurant
    {
        private int id;
        private string name;
        private string description;
        private string phoneNumber;
        private string street_name;
        private string street_number;
        private string postal_code;
        private string city;
        private string country;
        private Service lunchService;
        private Service dinnerService;
        private List<Meal> meals;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Restaurant Name")]
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

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [Required(ErrorMessage = "Street name is required.")]
        [Display(Name = "Street Name")]
        public string StreetName
        {
            get { return street_name; }
            set { street_name = value; }
        }

        [Required(ErrorMessage = "Street number is required.")]
        [Display(Name = "Street Number")]
        public string StreetNumber
        {
            get { return street_number; }
            set { street_number = value; }
        }

        [Required(ErrorMessage = "Postal code is required.")]
        [Display(Name = "Postal Code")]
        public string PostalCode
        {
            get { return postal_code; }
            set { postal_code = value; }
        }

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        [Required(ErrorMessage = "Lunch service is required.")]
        [Display(Name = "Lunch Service")]
        [ServiceTime(ErrorMessage = "Start time must be before end time.")]
        public Service LunchService
        {
            get { return lunchService; }
            set { lunchService = value; }
        }

        [Required(ErrorMessage = "Dinner service is required.")]
        [Display(Name = "Dinner Service")]
        [ServiceTime(ErrorMessage = "Start time must be before end time.")]
        public Service DinnerService
        {
            get { return dinnerService; }
            set { dinnerService = value; }
        }

        public List<Meal> Meals
        {
            get { return meals; }
            set { meals = value; }
        }

        public Restaurant(int id, string name, string description, string phoneNumber, string street_name, string street_number, string postal_code, string city, string country, Service lunchService, Service dinnerService)
        {
            Id = id;
            Name = name;
            Description = description;
            PhoneNumber = phoneNumber;
            StreetName = street_name;
            StreetNumber = street_number;
            PostalCode = postal_code;
            City = city;
            Country = country;
            LunchService = lunchService;
            DinnerService = dinnerService;
            Meals = new List<Meal>();
        }

        public void AddMeal(Meal meal)
        {
            if (meal != null && !Meals.Contains(meal))
                Meals.Add(meal);
        }


        public Restaurant()
        {
            Meals = new List<Meal>();
            LunchService = new Service();
            DinnerService = new Service();
        }

        public static async Task<List<Restaurant>> GetRestaurantsAsync(IRestaurantDAL restaurantDAL)
        {
            return await restaurantDAL.GetRestaurantsAsync();
        }

        public static async Task<Restaurant> GetRestaurantAsync(IRestaurantDAL restaurantDAL, int id, bool withMeals = false)
        {
            Restaurant restaurant = await restaurantDAL.GetRestaurantAsync(id, withMeals);
            return restaurant;
        }

        public async Task<bool> CreateAsync(IRestaurantDAL restaurantDAL, int userid)
        {
            return await restaurantDAL.InsertRestaurantAsync(this, userid);
        }
    }
}
