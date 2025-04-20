using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;


namespace TakeAway.DAL
{
    public class RestaurantDAL : IRestaurantDAL
    {
        private string connectionString;
        public RestaurantDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(IServiceDAL serviceDAL)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Restaurant r INNER JOIN Address a ON a.id_address = r.id_address", conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Restaurant r = new Restaurant();
                        r.Id = reader.GetInt32("id_restaurant");
                        r.Name = reader.GetString("name");
                        r.Description = reader.GetString("description");
                        r.PhoneNumber = reader.GetString("phoneNumber");
                        r.StreetName = reader.GetString("street_name");
                        r.StreetNumber = reader.GetString("street_number");
                        r.PostalCode = reader.GetString("postal_code");
                        r.City = reader.GetString("city");
                        r.Country = reader.GetString("country");
                        (Service lunchService, Service dinnerService) = await Service.GetServicesAsync(serviceDAL, r);
                        r.LunchService = lunchService;
                        r.DinnerService = dinnerService;
                        restaurants.Add(r);
                    }
                }
                return restaurants;
            }
        }
        public async Task<Restaurant> GetRestaurantAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

