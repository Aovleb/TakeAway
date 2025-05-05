using System.ComponentModel.DataAnnotations;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Client : User
    {
        private string lastName;
        private string firstName;
        private string phoneNumber;
        private string street_name;
        private string street_number;
        private string postal_code;
        private string city;
        private string country;
        private List<Meal> mealsInBasket;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        //[DataType(DataType.PhoneNumber)]
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
        public List<Meal> MealsInBasket
        {
            get { return mealsInBasket; }
            set { mealsInBasket = value; }
        }

        public Client(int id, string email, string password, string lastName, string firstName, string phoneNumber, string street_name, string street_number, string postal_code, string city, string country)
            : base(id, email, password)
        {
            LastName = lastName;
            FirstName = firstName;
            PhoneNumber = phoneNumber;
            StreetName = street_name;
            StreetNumber = street_number;
            PostalCode = postal_code;
            City = city;
            Country = country;
            MealsInBasket = new List<Meal>();
        }
        public Client() { MealsInBasket = new List<Meal>(); }

        public static async Task<Client> GetClientOfOrderAsync(IClientDAL clientDAL, Order order)
        {
            return await clientDAL.GetOrderAsync(order);
        }


    }
}
