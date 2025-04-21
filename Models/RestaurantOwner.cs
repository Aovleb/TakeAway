using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class RestaurantOwner : User
    {
        private string name;
        private List<Restaurant> restaurants;

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

        public RestaurantOwner(int id, string email, string password, string name, List<Restaurant> restaurants)
            : base(id, email, password)
        {
            Name = name;
            Restaurants = restaurants;
        }
        public RestaurantOwner() { }
    }
}
