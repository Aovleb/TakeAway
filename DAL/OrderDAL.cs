using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public async Task<bool> Create(Order order)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO ClientOrder (status, isDelivery, orderDate, id_person, id_service, id_restaurant)
                            VALUES (@status, @isDelivery, @orderDate, @id_person, @id_service, @id_restaurant);
                            SELECT SCOPE_IDENTITY();";

                        string insertOrderMealQuery = @"
                            INSERT INTO ClientOrder_Meal (orderNumber, id_meal, quantity)
                            VALUES (@orderNumber, @id_meal, @quantity);";

                        SqlCommand cmd = new SqlCommand(query, conn, transaction);
                        cmd.Parameters.AddWithValue("@status", (int)order.Status);
                        cmd.Parameters.AddWithValue("@isDelivery", order.IsDelivery);
                        cmd.Parameters.AddWithValue("@orderDate", order.Date);
                        cmd.Parameters.AddWithValue("@id_person", order.Client.Id);
                        cmd.Parameters.AddWithValue("@id_service", order.Service.Id);
                        cmd.Parameters.AddWithValue("@id_restaurant", order.Restaurant.Id);

                        decimal newOrderNumber = (decimal)await cmd.ExecuteScalarAsync();
                        int orderNumber = (int)newOrderNumber;

                        if (order.Meals == null || order.Meals.Count == 0)
                        {
                            transaction.Rollback();
                            return success;
                        }

                        foreach ((Meal meal, int quantity) in order.Meals)
                        {
                            SqlCommand mealCmd = new SqlCommand(insertOrderMealQuery, conn, transaction);
                            mealCmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                            mealCmd.Parameters.AddWithValue("@id_meal", meal.Id);
                            mealCmd.Parameters.AddWithValue("@quantity", quantity);
                            await mealCmd.ExecuteNonQueryAsync();
                        }

                        order.OrderNumber = orderNumber;

                        transaction.Commit();
                        success = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        success = false;
                    }
                }
            }
            return success;
        }


        public async Task<List<Order>> GetOrdersAsync(Restaurant restaurant)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT o.orderNumber, o.status, o.isDelivery, o.orderDate, o.id_person, o.id_service, o.id_restaurant,
                                        s.startTime AS serviceStartTime, s.endTime AS serviceEndTime,
                                        c.lastName, c.firstName, c.phoneNumber AS personPhoneNumber, p.email, p.password,
                                        caddr.street_name AS personStreetName, caddr.street_number AS personStreetNumber, 
                                        caddr.postal_code AS personPostalCode, caddr.city AS personCity, caddr.country AS personCountry,
                                        com.orderNumber AS mealOrderNumber, com.id_meal, com.quantity,
                                        m.name AS mealName, m.description AS mealDescription, m.price AS mealPrice,
                                        ms1.id_service AS meal_lunch_service_id, ms2.id_service AS meal_dinner_service_id,
                                        m2.id_meal AS menuId, d2.id_meal AS dishId,
                                        md.id_menu, md.id_dish,
                                        dm.id_meal AS dishMealId, dm.name AS dishName, dm.description AS dishDescription, dm.price AS dishPrice
                                FROM ClientOrder o
                                LEFT JOIN Service s ON s.id_service = o.id_service
                                LEFT JOIN Client c ON c.id_person = o.id_person
                                INNER JOIN Person p ON p.id_person = c.id_person
                                LEFT JOIN Address caddr ON caddr.id_address = c.id_address
                                LEFT JOIN ClientOrder_Meal com ON com.orderNumber = o.orderNumber
                                LEFT JOIN Meal m ON m.id_meal = com.id_meal
                                LEFT JOIN Meal_Service ms1 ON ms1.id_meal = m.id_meal AND ms1.id_service = (SELECT id_service FROM Service WHERE id_restaurant = o.id_restaurant AND startTime = (SELECT MIN(startTime) FROM Service WHERE id_restaurant = o.id_restaurant))
                                LEFT JOIN Meal_Service ms2 ON ms2.id_meal = m.id_meal AND ms2.id_service = (SELECT id_service FROM Service WHERE id_restaurant = o.id_restaurant AND startTime = (SELECT MAX(startTime) FROM Service WHERE id_restaurant = o.id_restaurant))
                                LEFT JOIN Menu m2 ON m2.id_meal = m.id_meal
                                LEFT JOIN Dish d2 ON d2.id_meal = m.id_meal
                                LEFT JOIN Menu_Dish md ON md.id_menu = m2.id_meal
                                LEFT JOIN Dish d ON d.id_meal = md.id_dish
                                LEFT JOIN Meal dm ON dm.id_meal = d.id_meal
                                WHERE o.id_restaurant = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", restaurant.Id);
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int orderNumber = reader.GetInt32("orderNumber");
                        Order order = orders.Find(o => o.OrderNumber == orderNumber);

                        if (order == null)
                        {
                            int status = reader.GetInt32("status");
                            bool isDelivery = reader.GetBoolean("isDelivery");
                            DateTime orderDate = reader.GetDateTime("orderDate");
                            int id_person = reader.GetInt32("id_person");
                            int id_service = reader.GetInt32("id_service");

                            TimeSpan serviceStartTime = reader.GetTimeSpan(reader.GetOrdinal("serviceStartTime"));
                            TimeSpan serviceEndTime = reader.GetTimeSpan(reader.GetOrdinal("serviceEndTime"));
                            Service orderService = new Service(id_service, serviceStartTime, serviceEndTime);

                            Client client = new Client(
                                id_person,
                                reader.GetString("lastName"),
                                reader.GetString("firstName"),
                                reader.GetString("personPhoneNumber"),
                                reader.GetString("email"),
                                reader.GetString("password"),
                                reader.GetString("personStreetName"),
                                reader.GetString("personStreetNumber"),
                                reader.GetString("personPostalCode"),
                                reader.GetString("personCity"),
                                reader.GetString("personCountry")
                            );

                            order = new Order(orderNumber, (StatusOrderEnum)status, isDelivery, orderDate, orderService, restaurant, client);
                            orders.Add(order);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("id_meal")))
                        {
                            int mealId = reader.GetInt32("id_meal");
                            string mealName = reader.GetString("mealName");
                            string mealDescription = reader.GetString("mealDescription");
                            decimal mealPrice = reader.GetDecimal("mealPrice");
                            int quantity = reader.GetInt32("quantity");

                            Service mealLunchService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("meal_lunch_service_id")))
                            {
                                int lunchServiceId = reader.GetInt32("meal_lunch_service_id");
                                mealLunchService = new Service(lunchServiceId, TimeSpan.Zero, TimeSpan.Zero);
                            }

                            Service mealDinnerService = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("meal_dinner_service_id")))
                            {
                                int dinnerServiceId = reader.GetInt32("meal_dinner_service_id");
                                mealDinnerService = new Service(dinnerServiceId, TimeSpan.Zero, TimeSpan.Zero);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("menuId")))
                            {
                                Menu menu = order.Meals.Keys.OfType<Menu>().FirstOrDefault(m => m.Id == mealId);
                                if (menu == null)
                                {
                                    menu = new Menu(mealId, mealName, mealDescription, mealPrice, mealLunchService, mealDinnerService);
                                    order.AddMeal(menu, quantity);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("dishMealId")))
                                {
                                    int dishMealId = reader.GetInt32("dishMealId");
                                    string dishName = reader.GetString("dishName");
                                    string dishDescription = reader.GetString("dishDescription");
                                    decimal dishPrice = reader.GetDecimal("dishPrice");

                                    if (!menu.Dishes.Any(d => d.Id == dishMealId))
                                    {
                                        Dish dish = new Dish(dishMealId, dishName, dishDescription, dishPrice, mealLunchService, mealDinnerService);
                                        menu.AddDish(dish);
                                    }
                                }
                            }
                            else if (!reader.IsDBNull(reader.GetOrdinal("dishId")))
                            {
                                if (!order.Meals.Keys.Any(m => m.Id == mealId))
                                {
                                    Dish dish = new Dish(mealId, mealName, mealDescription, mealPrice, mealLunchService, mealDinnerService);
                                    order.AddMeal(dish, quantity);
                                }
                            }
                        }
                    }
                }
            }

            return orders;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderNumber, StatusOrderEnum status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE ClientOrder SET status = @status WHERE orderNumber = @orderNumber", conn);
                cmd.Parameters.AddWithValue("@status", (int)status);
                cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
