using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class DishDAL : IDishDAL
    {
        private string connectionString;
        public DishDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Dish>> GetRestaurantDishesAsync(IServiceDAL serviceDAL, int id)
        {
            List<Dish> dishes = new List<Dish>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Dish d INNER JOIN Meal m ON m.id_meal = d.id_meal WHERE id_restaurant=@id;", conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        (Service lunchService, Service dinnerService) = await Service.GetMealServicesAsync(serviceDAL, dishId);
                        Dish d = new Dish(dishId, name, description, price, lunchService, dinnerService);
                        dishes.Add(d);
                    }
                }
                return dishes;
            }
        }

        public async Task<List<Dish>> GetDishesOfMenuAsync(int id)
        {
            List<Dish> dishes = new List<Dish>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM Dish d 
                                                  INNER JOIN Meal m ON m.id_meal = d.id_meal 
                                                  INNER JOIN Menu_Dish md ON md.id_menu = @id AND md.id_dish = m.id_meal;"
                                                , conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        Dish d = new Dish(dishId, name, description, price);
                        dishes.Add(d);
                    }
                }
                return dishes;
            }
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
    }
}
