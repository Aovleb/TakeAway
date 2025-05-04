using Microsoft.Data.SqlClient;
using System.Data;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;


namespace TakeAway.DAL
{
    public class RestaurantDAL : IRestaurantDAL
    {
        private string connectionString;
        public RestaurantDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(IServiceDAL serviceDAL, int id_restaurant = -1)
        {
            return id_restaurant == -1 ? await GetRestaurantsForClientAsync(serviceDAL) : await GetRestaurantsForRestaurateurAsync(serviceDAL, id_restaurant);
        }

        private async Task<List<Restaurant>> GetRestaurantsForClientAsync(IServiceDAL serviceDAL)
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

        private async Task<List<Restaurant>> GetRestaurantsForRestaurateurAsync(IServiceDAL serviceDAL, int restaurateurId)
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Restaurant r INNER JOIN Address a ON a.id_address = r.id_address WHERE id_person = @id_person", conn);
                cmd.Parameters.AddWithValue("@id_person", restaurateurId);
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


        public async Task<Restaurant> GetRestaurantAsync(IServiceDAL serviceDAL,IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL, int id)
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

                        List<Meal> meals = await Meal.GetRestaurantMealsAsync(mealDAL, menuDAL, dishDAL, serviceDAL, id);

                        meals.ForEach(m => r.AddMeal(m));
                    }
                }
                return r;
            }
        }

        public async Task<bool> InsertRestaurantAsync(IServiceDAL serviceDAL, Restaurant restaurant, int userId)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert Address
                    string addressQuery = @"INSERT INTO Address (street_name, street_number, postal_code, city, country)
                                    OUTPUT INSERTED.id_address
                                    VALUES (@StreetName, @StreetNumber, @PostalCode, @City, @Country)";

                    SqlCommand addressCmd = new SqlCommand(addressQuery, conn, transaction);
                    addressCmd.Parameters.AddWithValue("@StreetName", restaurant.StreetName);
                    addressCmd.Parameters.AddWithValue("@StreetNumber", restaurant.StreetNumber);
                    addressCmd.Parameters.AddWithValue("@PostalCode", restaurant.PostalCode);
                    addressCmd.Parameters.AddWithValue("@City", restaurant.City);
                    addressCmd.Parameters.AddWithValue("@Country", restaurant.Country);

                    int addressId = (int)await addressCmd.ExecuteScalarAsync();

                    // Insert Restaurant
                    string restaurantQuery = @"INSERT INTO Restaurant (name, description, phoneNumber, id_address, id_person)
                                       OUTPUT INSERTED.id_restaurant
                                       VALUES (@Name, @Description, @Phone, @AddressId, @PersonId)";

                    SqlCommand restaurantCmd = new SqlCommand(restaurantQuery, conn, transaction);
                    restaurantCmd.Parameters.AddWithValue("@Name", restaurant.Name);
                    restaurantCmd.Parameters.AddWithValue("@Description", restaurant.Description);
                    restaurantCmd.Parameters.AddWithValue("@Phone", restaurant.PhoneNumber);
                    restaurantCmd.Parameters.AddWithValue("@AddressId", addressId);
                    restaurantCmd.Parameters.AddWithValue("@PersonId", userId);

                    int restaurantId = (int)await restaurantCmd.ExecuteScalarAsync();

                    // Insert Services
                    bool lunchRes = await serviceDAL.InsertService(restaurant.LunchService, restaurantId, conn, transaction);
                    bool dinnerRes = await serviceDAL.InsertService(restaurant.DinnerService, restaurantId, conn, transaction);

                    if (lunchRes && dinnerRes)
                    {
                        transaction.Commit();
                        success = true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return success;
        }


    }
}

