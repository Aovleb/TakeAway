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

        public RestaurantOwner(int id, string email, string password, string name)
            : base(id, email, password)
        {
            Name = name;
            Restaurants = restaurants;
            Restaurants = new List<Restaurant>();
        }
        public RestaurantOwner() { }

        public async Task<bool> CreateAsync(IUserDAL userDAL)
        {
            return await userDAL.CreateAsync(this);
        }
    }
}
