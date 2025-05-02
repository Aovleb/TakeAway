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
                        int restaurantId = reader.GetInt32("id_restaurant");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        string phoneNumber = reader.GetString("phoneNumber");
                        string streetName = reader.GetString("street_name");
                        string streetNumber = reader.GetString("street_number");
                        string postalCode = reader.GetString("postal_code");
                        string city = reader.GetString("city");
                        string country = reader.GetString("country");
                        (Service lunchService, Service dinnerService) = await Service.GetRestaurantServicesAsync(serviceDAL, restaurantId);
                        Restaurant r = new Restaurant(restaurantId, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                        restaurants.Add(r);
                    }
                }
                return restaurants;
            }
        }
        public async Task<Restaurant> GetRestaurantAsync(IServiceDAL serviceDAL, int id)
        {
            Restaurant r = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Restaurant r INNER JOIN Address a ON a.id_address = r.id_address WHERE id_restaurant = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        string phoneNumber = reader.GetString("phoneNumber");
                        string streetName = reader.GetString("street_name");
                        string streetNumber = reader.GetString("street_number");
                        string postalCode = reader.GetString("postal_code");
                        string city = reader.GetString("city");
                        string country = reader.GetString("country");
                        (Service lunchService, Service dinnerService) = await Service.GetRestaurantServicesAsync(serviceDAL, id);
                        r = new Restaurant(id, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                    }
                }
                return r;
            }
        }
    }
}

