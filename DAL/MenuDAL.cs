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
                        Menu m = new Menu(menuId, name, description, price, lunchService, dinnerService, dishes);
                        menus.Add(m);
                    }
                }
                return menus;
            }
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
                        foreach(Dish dish in menu.Dishes)
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