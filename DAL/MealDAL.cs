using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;


namespace TakeAway.DAL
{
    public class MealDAL : IMealDAL
    {
        private string connectionString;
        public MealDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Meal> GetMealAsync(IServiceDAL serviceDAL, IDishDAL dishDAL, int mealId)
        {
            Meal meal = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                //Dish
                SqlCommand cmd = new SqlCommand("SELECT * FROM meal m INNER JOIN dish d ON m.id_meal = d.id_meal WHERE d.id_meal = @mealId", conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        (Service lunchService, Service dinnerService) = await Service.GetMealServicesAsync(serviceDAL, dishId);
                        meal = new Dish(dishId, name, description, price, lunchService, dinnerService);
                    }
                }
                if (meal != null) return meal;

                //menu
                cmd = new SqlCommand("SELECT * FROM meal m INNER JOIN menu me ON m.id_meal = me.id_meal WHERE me.id_meal = @mealId", conn);
                cmd.Parameters.AddWithValue("@mealId", mealId);

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

                        meal = new Menu(menuId, name, description, price, lunchService, dinnerService, dishes);
                    }
                }
            }
            return meal;
        }

        public async Task<List<Meal>> GetRestaurantMealsAsync(IMenuDAL menuDAL, IDishDAL dishDAL, IServiceDAL serviceDAL, int id)
        {
            List<Meal> meals = new List<Meal>();
            List<Menu> menus = await Menu.GetRestaurantMenusAsync(menuDAL, dishDAL, serviceDAL, id);
            meals.AddRange(menus);
            List<Dish> dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, id);
            meals.AddRange(dishes);
            return meals;
        }

        public async Task<List<Meal>> GetOrderMealsAsync(int id, IDishDAL dishDAL, IServiceDAL serviceDAL)
        {
            List<Meal> meals = new List<Meal>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                //Dish
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM meal m 
                                                  INNER JOIN dish d ON m.id_meal = d.id_meal 
                                                  INNER JOIN ClientOrder_Meal co ON m.id_meal = co.id_meal
                                                  WHERE co.orderNumber = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int dishId = reader.GetInt32("id_meal");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        Dish d = new Dish(dishId, name, description, price);
                        meals.Add(d);
                    }
                }
                //menu
                cmd = new SqlCommand(@"SELECT * FROM meal m 
                                       INNER JOIN menu me ON m.id_meal = me.id_meal 
                                       INNER JOIN ClientOrder_Meal co ON m.id_meal = co.id_meal
                                       WHERE co.orderNumber = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

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

                        Menu m = new Menu(menuId, name, description, price, lunchService, dinnerService, dishes);
                        meals.Add(m);
                    }
                }
            }
            return meals;
        }
    }
}
    