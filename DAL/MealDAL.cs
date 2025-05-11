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

        public async Task<List<Meal>> GetOrderMealsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddInBasket(Meal m, int clientId)
        {
            bool success = false;
            string verifyMealQuery = @"SELECT * FROM client_meal WHERE id_person = @id_person AND id_meal = @id_meal";
            string insertClientMealQuery = @"INSERT INTO client_meal (id_person, id_meal) 
                                                   VALUES (@id_person, @id_meal)";
            string incrementQuantityQuery = @"UPDATE client_meal SET quantity = quantity + 1 WHERE id_person = @id_person AND id_meal = @id_meal";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    //verify if the meal is already in the basket
                    SqlCommand cmd = new SqlCommand(verifyMealQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_person", clientId);
                    cmd.Parameters.AddWithValue("@id_meal", m.Id);
                    int cpt = 0;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cpt++;                            
                        }
                    }
                    int rows;

                    if (cpt > 0)
                    {
                        cmd = new SqlCommand(incrementQuantityQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_person", clientId);
                        cmd.Parameters.AddWithValue("@id_meal", m.Id);
                        rows = await cmd.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        //insert the meal
                        cmd = new SqlCommand(insertClientMealQuery, conn, transaction);
                        cmd.Parameters.AddWithValue("@id_person", clientId);
                        cmd.Parameters.AddWithValue("@id_meal", m.Id);
                        rows = await cmd.ExecuteNonQueryAsync();
                    }                        

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

        public async Task<bool> RemoveFromBasket(Meal m, int clientId)
        {
            bool success = false;
            string verifyMealQuery = @"SELECT * FROM client_meal WHERE id_person = @id_person AND id_meal = @id_meal";
            string RemoveClientMealQuery = @"DELETE FROM client_meal WHERE id_person = @id_person AND id_meal = @id_meal";
            string decrementQuantityQuery = @"UPDATE client_meal SET quantity = quantity - 1 WHERE id_person = @id_person AND id_meal = @id_meal";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    //verify if the meal is in the basket
                    SqlCommand cmd = new SqlCommand(verifyMealQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_person", clientId);
                    cmd.Parameters.AddWithValue("@id_meal", m.Id);
                    int cpt = 0;
                    int quantity = 0;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cpt++;
                            quantity = reader.GetInt32("quantity");
                        }
                    }
                    int rows;

                    //if the meal is in the basket
                    if (cpt > 0)
                    {
                        if(quantity <=1)
                        {
                            //remove the meal
                            cmd = new SqlCommand(RemoveClientMealQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@id_person", clientId);
                            cmd.Parameters.AddWithValue("@id_meal", m.Id);
                            rows = await cmd.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            //decrement the quantity
                            cmd = new SqlCommand(decrementQuantityQuery, conn, transaction);
                            cmd.Parameters.AddWithValue("@id_person", clientId);
                            cmd.Parameters.AddWithValue("@id_meal", m.Id);
                            rows = await cmd.ExecuteNonQueryAsync();
                        }
                        if (rows > 0)
                        {
                            transaction.Commit();
                            success = true;
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
    