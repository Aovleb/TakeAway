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

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            List<Restaurant> restaurants = new List<Restaurant>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT r.id_restaurant, r.name, r.description, r.phoneNumber, 
                            a.street_name, a.street_number, a.postal_code, a.city, a.country,
                            s1.id_service AS lunch_service_id, s1.startTime AS lunch_startTime, s1.endTime AS lunch_endTime,
                            s2.id_service AS dinner_service_id, s2.startTime AS dinner_startTime, s2.endTime AS dinner_endTime
                      FROM Restaurant r
                      INNER JOIN Address a ON a.id_address = r.id_address
                      LEFT JOIN Service s1 ON s1.id_restaurant = r.id_restaurant AND s1.startTime = (SELECT MIN(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
                      LEFT JOIN Service s2 ON s2.id_restaurant = r.id_restaurant AND s2.startTime = (SELECT MAX(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)",
                    conn);

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

                        Service lunchService = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("lunch_service_id")))
                        {
                            int lunchServiceId = reader.GetInt32("lunch_service_id");
                            TimeSpan lunchStartTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_startTime"));
                            TimeSpan lunchEndTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_endTime"));
                            lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                        }

                        Service dinnerService = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("dinner_service_id")))
                        {
                            int dinnerServiceId = reader.GetInt32("dinner_service_id");
                            TimeSpan dinnerStartTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_startTime"));
                            TimeSpan dinnerEndTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_endTime"));
                            dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);
                        }

                        Restaurant r = new Restaurant(restaurantId, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                        restaurants.Add(r);
                    }
                }
            }
            return restaurants;
        }


        public async Task<Restaurant> GetRestaurantAsync(int id, bool withMeals = false)
        {
            return withMeals ? await GetRestaurantWithMealsAsync(id) : await GetRestaurantWithOutMealsAsync(id);
        }

        private async Task<Restaurant> GetRestaurantWithOutMealsAsync(int id)
        {
            Restaurant r = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT r.id_restaurant, r.name, r.description, r.phoneNumber,
                   a.street_name, a.street_number, a.postal_code, a.city, a.country,
                   s1.id_service AS lunch_service_id, s1.startTime AS lunch_startTime, s1.endTime AS lunch_endTime,
                   s2.id_service AS dinner_service_id, s2.startTime AS dinner_startTime, s2.endTime AS dinner_endTime
            FROM Restaurant r
            INNER JOIN Address a ON a.id_address = r.id_address
            LEFT JOIN Service s1 ON s1.id_restaurant = r.id_restaurant AND s1.startTime = (SELECT MIN(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
            LEFT JOIN Service s2 ON s2.id_restaurant = r.id_restaurant AND s2.startTime = (SELECT MAX(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
            WHERE r.id_restaurant = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (r == null)
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

                            Service lunchService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lunch_service_id")))
                            {
                                int lunchServiceId = reader.GetInt32("lunch_service_id");
                                TimeSpan lunchStartTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_startTime"));
                                TimeSpan lunchEndTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_endTime"));
                                lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                            }

                            Service dinnerService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("dinner_service_id")))
                            {
                                int dinnerServiceId = reader.GetInt32("dinner_service_id");
                                TimeSpan dinnerStartTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_startTime"));
                                TimeSpan dinnerEndTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_endTime"));
                                dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);
                            }

                            r = new Restaurant(restaurantId, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                        }
                    }
                }
            }

            return r;
        }

        private async Task<Restaurant> GetRestaurantWithMealsAsync(int id)
        {
            Restaurant r = null;
            Menu currentMenu = null;
            int? lastMenuId = null; // Pour suivre le dernier menu traité

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                        SELECT 
                            r.id_restaurant, r.name, r.description, r.phoneNumber,
                            a.street_name, a.street_number, a.postal_code, a.city, a.country,
                            s1.id_service AS lunch_service_id, s1.startTime AS lunch_startTime, s1.endTime AS lunch_endTime,
                            s2.id_service AS dinner_service_id, s2.startTime AS dinner_startTime, s2.endTime AS dinner_endTime,
                            m.id_meal, m.name AS mealName, m.description AS mealDescription, m.price AS mealPrice,
                            ms1.id_service AS meal_lunch_service_id, ms2.id_service AS meal_dinner_service_id,
                            m2.id_meal AS menuId, d2.id_meal AS dishId,
                            md.id_menu, md.id_dish,
                            dm.id_meal AS dishMealId, dm.name AS dishName, dm.description AS dishDescription, dm.price AS dishPrice
                        FROM Restaurant r
                        INNER JOIN Address a ON a.id_address = r.id_address
                        LEFT JOIN Service s1 ON s1.id_restaurant = r.id_restaurant 
                            AND s1.startTime = (SELECT MIN(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
                        LEFT JOIN Service s2 ON s2.id_restaurant = r.id_restaurant 
                            AND s2.startTime = (SELECT MAX(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
                        LEFT JOIN Meal m ON m.id_restaurant = r.id_restaurant
                        LEFT JOIN Meal_Service ms1 ON ms1.id_meal = m.id_meal AND ms1.id_service = s1.id_service
                        LEFT JOIN Meal_Service ms2 ON ms2.id_meal = m.id_meal AND ms2.id_service = s2.id_service
                        LEFT JOIN Menu m2 ON m2.id_meal = m.id_meal
                        LEFT JOIN Dish d2 ON d2.id_meal = m.id_meal
                        LEFT JOIN Menu_Dish md ON md.id_menu = m.id_meal
                        LEFT JOIN Dish d ON d.id_meal = md.id_dish
                        LEFT JOIN Meal dm ON dm.id_meal = d.id_meal
                        WHERE r.id_restaurant = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (r == null)
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

                            Service lunchService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("lunch_service_id")))
                            {
                                int lunchServiceId = reader.GetInt32("lunch_service_id");
                                TimeSpan lunchStartTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_startTime"));
                                TimeSpan lunchEndTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_endTime"));
                                lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                            }

                            Service dinnerService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("dinner_service_id")))
                            {
                                int dinnerServiceId = reader.GetInt32("dinner_service_id");
                                TimeSpan dinnerStartTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_startTime"));
                                TimeSpan dinnerEndTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_endTime"));
                                dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);
                            }

                            r = new Restaurant(restaurantId, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("id_meal")))
                        {
                            int mealId = reader.GetInt32("id_meal");
                            string mealName = reader.GetString("mealName");
                            string mealDescription = reader.GetString("mealDescription");
                            decimal mealPrice = reader.GetDecimal("mealPrice");

                            // Déterminer les services associés au repas
                            Service mealLunchService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("meal_lunch_service_id")))
                            {
                                mealLunchService = r.LunchService; // Service déjeuner associé via Meal_Service
                            }

                            Service mealDinnerService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("meal_dinner_service_id")))
                            {
                                mealDinnerService = r.DinnerService; // Service dîner associé via Meal_Service
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("menuId")))
                            {
                                // C'est un menu
                                if (lastMenuId != mealId)
                                {
                                    currentMenu = new Menu(mealId, mealName, mealDescription, mealPrice, mealLunchService, mealDinnerService);
                                    r.AddMeal(currentMenu);
                                    lastMenuId = mealId;
                                }

                                // Vérifier si un plat est associé au menu via Menu_Dish
                                if (!reader.IsDBNull(reader.GetOrdinal("dishMealId")))
                                {
                                    int dishMealId = reader.GetInt32("dishMealId");
                                    string dishName = reader.GetString("dishName");
                                    string dishDescription = reader.GetString("dishDescription");
                                    decimal dishPrice = reader.GetDecimal("dishPrice");

                                    Dish dish = new Dish(dishMealId, dishName, dishDescription, dishPrice, mealLunchService, mealDinnerService);
                                    currentMenu.AddDish(dish);
                                }
                            }
                            else if (!reader.IsDBNull(reader.GetOrdinal("dishId")))
                            {
                                // C'est un plat indépendant
                                Dish d = new Dish(mealId, mealName, mealDescription, mealPrice, mealLunchService, mealDinnerService);
                                r.AddMeal(d);
                            }
                        }
                    }
                }
            }
            return r;
        }
        public async Task<bool> InsertRestaurantAsync(Restaurant restaurant, int userId)
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
                    string serviceQuery = @"INSERT INTO Service (startTime, endTime, id_restaurant)
                                   VALUES (@StartTime, @EndTime, @RestaurantId)";

                    // Insertion du service déjeuner
                    SqlCommand lunchCmd = new SqlCommand(serviceQuery, conn, transaction);
                    lunchCmd.Parameters.AddWithValue("@StartTime", restaurant.LunchService.StartTime);
                    lunchCmd.Parameters.AddWithValue("@EndTime", restaurant.LunchService.EndTime);
                    lunchCmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
                    int lunchRows = await lunchCmd.ExecuteNonQueryAsync();

                    // Insertion du service dîner
                    SqlCommand dinnerCmd = new SqlCommand(serviceQuery, conn, transaction);
                    dinnerCmd.Parameters.AddWithValue("@StartTime", restaurant.DinnerService.StartTime);
                    dinnerCmd.Parameters.AddWithValue("@EndTime", restaurant.DinnerService.EndTime);
                    dinnerCmd.Parameters.AddWithValue("@RestaurantId", restaurantId);
                    int dinnerRows = await dinnerCmd.ExecuteNonQueryAsync();

                    if (lunchRows == 1 && dinnerRows == 1)
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

