using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class MenuDAL : IMenuDAL
    {
        private string connectionString;
        public MenuDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Menu>> GetRestaurantMenusAsync(IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            List<Menu> menus = new List<Menu>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Menu m INNER JOIN Meal ml ON m.id_meal = ml.id_meal WHERE id_restaurant=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        int menuId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");
                        List<Dish> dishes = await Dish.GetDishesOfMenuAsync(dishDAL, menuId);
                        (Service lunchService, Service dinnerService) = await Service.GetMealServicesAsync(serviceDAL, menuId);
                        Menu m = new Menu(menuId, name, description, price, dishes, lunchService, dinnerService);
                        menus.Add(m);
                    }
                }
                return menus;
            }
        }
    }
}
