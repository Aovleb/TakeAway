using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [Required(ErrorMessage = "Street name is required.")]
        [Display(Name = "Street Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street name can only contain letters, and spaces.")]
        public string StreetName
        {
            get { return street_name; }
            set { street_name = value; }
        }

        [Required(ErrorMessage = "Street number is required.")]
        [Display(Name = "Street Number")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Street number can only contain digits and letters.")]
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
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Country can only contain letters and spaces.")]
        public string Country
        {
            get { return country; }
            set { country = value; }
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
        }
        public Client() { }


        public async override Task<bool> CreateAsync(IUserDAL userDAL)
        {
            return await userDAL.CreateAsync(this);
        }

        public async static Task<Client> GetClientAsync(IUserDAL userDAL, int id)
        {
            return await userDAL.GetClientAsync(id);
        }
    }
}
