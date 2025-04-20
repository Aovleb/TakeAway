using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
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
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string StreetName
        {
            get { return street_name; }
            set { street_name = value; }
        }
        public string StreetNumber
        {
            get { return street_number; }
            set { street_number = value; }
        }
        public string PostalCode
        {
            get { return postal_code; }
            set { postal_code = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
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
        public List<Meal> Meals
        {
            get { return meals; }
            set { meals = value; }
        }

        public Restaurant(int id, string name, string description, string phoneNumber, string street_name, string street_number, string postal_code, string city, string country, Service lunchService, Service dinnerService, List<Meal> meals)
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
            Meals = meals;
        }

        public Restaurant() { }

        public static async Task<List<Restaurant>> GetRestaurantsAsync(IRestaurantDAL restaurantDAL, IServiceDAL serviceDdAL)
        {
            return await restaurantDAL.GetRestaurantsAsync(serviceDdAL);
        }
    }
}
