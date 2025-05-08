using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class ClientDAL : IClientDAL
    {
        private string connectionString;
        public ClientDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Meal>> GetBasketAsync(IServiceDAL serviceDAL, IDishDAL dishDAL, int clientId)
        {
            List<Meal> meals = new List<Meal>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //dish
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM meal m
                                                INNER JOIN dish d ON m.id_meal = d.id_meal
                                                INNER JOIN client_meal cl ON id_person = @userId AND cl.id_meal = d.id_meal", conn);
                cmd.Parameters.AddWithValue("@userId", clientId);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");
                        int quantity = reader.GetInt32("quantity");

                        (Service lunchService, Service dinnerService) = await Service.GetMealServicesAsync(serviceDAL, dishId);
                        Dish d = new Dish(dishId, name, description, price, lunchService, dinnerService);

                        for(int i = 0; i < quantity; i++)
                        {
                           meals.Add(d);
                        }
                    }
                }

                //menu
                cmd = new SqlCommand(@"SELECT * FROM meal m
                                                INNER JOIN menu me ON m.id_meal = me.id_meal
                                                INNER JOIN client_meal cl ON id_person = @userId AND cl.id_meal = me.id_meal", conn);
                cmd.Parameters.AddWithValue("@userId", clientId);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int menuId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");
                        int quantity = reader.GetInt32("quantity");

                        List<Dish> dishes = await Dish.GetDishesOfMenuAsync(dishDAL, menuId);
                        (Service lunchService, Service dinnerService) = await Service.GetMealServicesAsync(serviceDAL, menuId);

                        Menu m = new Menu(menuId, name, description, price, lunchService, dinnerService, dishes);

                        for (int i = 0; i < quantity; i++)
                        {
                            meals.Add(m);
                        }
                        
                    }
                }
            }
            return meals;
        }

        public async Task<Client> GetOrderAsync(Order o)
        {
            throw new NotImplementedException();
        }
    }
}
