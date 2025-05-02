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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Dish d INNER JOIN Meal m ON m.id_meal = d.id_meal INNER JOIN Menu_Dish md ON md.id_menu = @id AND md.id_dish = m.id_meal;", conn);
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
    }
}
