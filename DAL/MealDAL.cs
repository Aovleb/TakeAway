using Microsoft.Data.SqlClient;
using System.Data;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;


namespace TakeAway.DAL
{
    public class MealDAL : IMealDAL
    {
        private string connectionString;
        public MealDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Meal> GetMealAsync(int mealId)
        {
            Meal meal = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
            SELECT m.id_meal, m.name, m.description, m.price
            FROM Meal m
            INNER JOIN Dish d ON m.id_meal = d.id_meal
            WHERE m.id_meal = @mealId", conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        await reader.CloseAsync();
                        cmd = new SqlCommand(@"
                    SELECT s.id_service, s.startTime, s.endTime
                    FROM Meal_Service ms
                    INNER JOIN Service s ON ms.id_service = s.id_service
                    WHERE ms.id_meal = @mealId
                    ORDER BY s.startTime", conn);
                        cmd.Parameters.AddWithValue("@mealId", mealId);

                        Service lunchService = null;
                        Service dinnerService = null;
                        using (SqlDataReader serviceReader = await cmd.ExecuteReaderAsync())
                        {
                            if (await serviceReader.ReadAsync())
                            {
                                int lunchServiceId = serviceReader.GetInt32("id_service");
                                TimeSpan lunchStartTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("startTime"));
                                TimeSpan lunchEndTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("endTime"));
                                lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                            }

                            if (await serviceReader.ReadAsync())
                            {
                                int dinnerServiceId = serviceReader.GetInt32("id_service");
                                TimeSpan dinnerStartTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("startTime"));
                                TimeSpan dinnerEndTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("endTime"));
                                dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);
                            }
                        }

                        meal = new Dish(dishId, name, description, price, lunchService, dinnerService);
                    }
                }

                if (meal != null) return meal;

                cmd = new SqlCommand(@"
            SELECT m.id_meal, m.name, m.description, m.price
            FROM Meal m
            INNER JOIN Menu me ON m.id_meal = me.id_meal
            WHERE m.id_meal = @mealId", conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int menuId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        await reader.CloseAsync();
                        cmd = new SqlCommand(@"
                    SELECT s.id_service, s.startTime, s.endTime
                    FROM Meal_Service ms
                    INNER JOIN Service s ON ms.id_service = s.id_service
                    WHERE ms.id_meal = @mealId
                    ORDER BY s.startTime", conn);
                        cmd.Parameters.AddWithValue("@mealId", menuId);

                        Service lunchService = null;
                        Service dinnerService = null;
                        using (SqlDataReader serviceReader = await cmd.ExecuteReaderAsync())
                        {
                            if (await serviceReader.ReadAsync())
                            {
                                int lunchServiceId = serviceReader.GetInt32("id_service");
                                TimeSpan lunchStartTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("startTime"));
                                TimeSpan lunchEndTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("endTime"));
                                lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                            }

                            if (await serviceReader.ReadAsync())
                            {
                                int dinnerServiceId = serviceReader.GetInt32("id_service");
                                TimeSpan dinnerStartTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("startTime"));
                                TimeSpan dinnerEndTime = serviceReader.GetTimeSpan(serviceReader.GetOrdinal("endTime"));
                                dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);
                            }
                        }

                        Menu menu = new Menu(menuId, name, description, price, lunchService, dinnerService);

                        await reader.CloseAsync();
                        cmd = new SqlCommand(@"
                    SELECT m.id_meal, m.name, m.description, m.price
                    FROM Meal m
                    INNER JOIN Dish d ON m.id_meal = d.id_meal
                    INNER JOIN Menu_Dish md ON d.id_meal = md.id_dish
                    WHERE md.id_menu = @menuId", conn);
                        cmd.Parameters.AddWithValue("@menuId", menuId);

                        List<(int id, string name, string description, decimal price)> tempDishes = new List<(int, string, string, decimal)>();
                        using (SqlDataReader dishReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dishReader.ReadAsync())
                            {
                                int dishId = dishReader.GetInt32("id_meal");
                                string dishName = dishReader.GetString("name");
                                string dishDescription = dishReader.GetString("description");
                                decimal dishPrice = dishReader.GetDecimal("price");
                                tempDishes.Add((dishId, dishName, dishDescription, dishPrice));
                            }
                        }

                        foreach (var tempDish in tempDishes)
                        {
                            int dishId = tempDish.id;
                            string dishName = tempDish.name;
                            string dishDescription = tempDish.description;
                            decimal dishPrice = tempDish.price;

                            cmd = new SqlCommand(@"
                        SELECT s.id_service, s.startTime, s.endTime
                        FROM Meal_Service ms
                        INNER JOIN Service s ON ms.id_service = s.id_service
                        WHERE ms.id_meal = @dishId
                        ORDER BY s.startTime", conn);
                            cmd.Parameters.AddWithValue("@dishId", dishId);

                            Service dishLunchService = null;
                            Service dishDinnerService = null;
                            using (SqlDataReader dishServiceReader = await cmd.ExecuteReaderAsync())
                            {
                                if (await dishServiceReader.ReadAsync())
                                {
                                    int dishLunchServiceId = dishServiceReader.GetInt32("id_service");
                                    TimeSpan dishLunchStartTime = dishServiceReader.GetTimeSpan(dishServiceReader.GetOrdinal("startTime"));
                                    TimeSpan dishLunchEndTime = dishServiceReader.GetTimeSpan(dishServiceReader.GetOrdinal("endTime"));
                                    dishLunchService = new Service(dishLunchServiceId, dishLunchStartTime, dishLunchEndTime);
                                }

                                if (await dishServiceReader.ReadAsync())
                                {
                                    int dishDinnerServiceId = dishServiceReader.GetInt32("id_service");
                                    TimeSpan dishDinnerStartTime = dishServiceReader.GetTimeSpan(dishServiceReader.GetOrdinal("startTime"));
                                    TimeSpan dishDinnerEndTime = dishServiceReader.GetTimeSpan(dishServiceReader.GetOrdinal("endTime"));
                                    dishDinnerService = new Service(dishDinnerServiceId, dishDinnerStartTime, dishDinnerEndTime);
                                }
                            }

                            Dish dish = new Dish(dishId, dishName, dishDescription, dishPrice, dishLunchService, dishDinnerService);
                            menu.AddDish(dish);
                        }

                        meal = menu;
                    }
                }
            }
            return meal;
        }

        public async Task<bool> CreateAsync(Dish dish, int restaurantId)
        {
            bool success = false;
            string mealQuery = @"INSERT INTO meal (name, description, price, id_restaurant) OUTPUT INSERTED.id_meal 
                                           VALUES (@name, @description, @price, @id_restaurant)";
            string mealServiceQuery = @"INSERT INTO Meal_Service(id_service,id_meal) VALUES (@id_service, @id_meal)";
            string dishQuery = @"INSERT INTO Dish(id_meal) VALUES (@id_meal)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(mealQuery, conn, transaction);

                    cmd.Parameters.AddWithValue("@name", dish.Name);
                    cmd.Parameters.AddWithValue("@description", dish.Description);
                    cmd.Parameters.AddWithValue("@price", dish.Price);
                    cmd.Parameters.AddWithValue("@id_restaurant", restaurantId);

                    int mealId = (int)await cmd.ExecuteScalarAsync();


                    if (dish.LunchService != null)
                    {
                        cmd = new SqlCommand(mealServiceQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_service", dish.LunchService.Id);
                        cmd.Parameters.AddWithValue("@id_meal", mealId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    if (dish.DinnerService != null)
                    {
                        cmd = new SqlCommand(mealServiceQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_service", dish.DinnerService.Id);
                        cmd.Parameters.AddWithValue("@id_meal", mealId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    cmd = new SqlCommand(dishQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_meal", mealId);
                    int rows = await cmd.ExecuteNonQueryAsync();

                    if (rows > 0)
                    {
                        transaction.Commit();
                        success = true;
                    }
                    else
                        transaction.Rollback();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return success;
        }

        public async Task<bool> CreateAsync(Menu menu, int restaurantId)
        {
            bool success = false;
            string mealQuery = @"INSERT INTO meal (name, description, price, id_restaurant) OUTPUT INSERTED.id_meal 
                                           VALUES (@name, @description, @price, @id_restaurant)";
            string mealServiceQuery = @"INSERT INTO Meal_Service(id_service,id_meal) VALUES (@id_service, @id_meal)";
            string menuQuery = @"INSERT INTO Menu(id_meal) VALUES (@id_meal)";
            string menuDishQuery = @"INSERT INTO Menu_Dish(id_menu,id_dish) VALUES (@id_menu, @id_dish)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(mealQuery, conn, transaction);

                    cmd.Parameters.AddWithValue("@name", menu.Name);
                    cmd.Parameters.AddWithValue("@description", menu.Description);
                    cmd.Parameters.AddWithValue("@price", menu.Price);
                    cmd.Parameters.AddWithValue("@id_restaurant", restaurantId);

                    int mealId = (int)await cmd.ExecuteScalarAsync();


                    if (menu.LunchService != null)
                    {
                        cmd = new SqlCommand(mealServiceQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_service", menu.LunchService.Id);
                        cmd.Parameters.AddWithValue("@id_meal", mealId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    if (menu.DinnerService != null)
                    {
                        cmd = new SqlCommand(mealServiceQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_service", menu.DinnerService.Id);
                        cmd.Parameters.AddWithValue("@id_meal", mealId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    cmd = new SqlCommand(menuQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_meal", mealId);

                    int rows = await cmd.ExecuteNonQueryAsync();

                    if (rows > 0)
                    {
                        int res = 0;
                        foreach (Dish dish in menu.Dishes)
                        {
                            cmd = new SqlCommand(menuDishQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@id_menu", mealId);
                            cmd.Parameters.AddWithValue("@id_dish", dish.Id);
                            res += await cmd.ExecuteNonQueryAsync();
                        }
                        if (res == menu.Dishes.Count)
                        {
                            success = true;
                            transaction.Commit();
                        }
                        else
                            transaction.Rollback();
                    }
                    else
                        transaction.Rollback();
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
