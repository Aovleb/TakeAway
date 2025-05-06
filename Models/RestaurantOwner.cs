using System.ComponentModel.DataAnnotations;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class RestaurantOwner : User
    {
        private string name;
        private List<Restaurant> restaurants;


        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Restaurant Owner Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Restaurant> Restaurants
        {
            get { return restaurants; }
            set { restaurants = value; }
        }

        public RestaurantOwner(int id, string email, string password, string name)
            : base(id, email, password)
        {
            Name = name;
            Restaurants = new List<Restaurant>();
        }
        public RestaurantOwner() { Restaurants = new List<Restaurant>(); }

        public async Task<bool> CreateAsync(IUserDAL userDAL)
        {
            return await userDAL.CreateAsync(this);
        }
    }
}
