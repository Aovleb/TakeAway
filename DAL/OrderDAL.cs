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

        public async Task<List<Order>> GetOrdersAsync(int id, IClientDAL clientDAL, IServiceDAL serviceDAL, IMealDAL mealDAL, IRestaurantDAL restaurantDAL, IDishDAL dishDAL)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ClientOrder o WHERE id_restaurant = @id", conn);
                await conn.OpenAsync();
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int orderNumber = reader.GetInt32("orderNumber");
                        int status = reader.GetInt32("status");
                        bool isDelivery = reader.GetBoolean("isDelivery");
                        DateTime orderDate = reader.GetDateTime("orderDate");
                        int id_person = reader.GetInt32("id_person");
                        int id_service = reader.GetInt32("id_service");
                        Service lunchService = await Service.GetOrderServiceAsync(serviceDAL, id_service);
                        Client client = await Client.GetClientOfOrderAsync(clientDAL, id_person);
                        Restaurant r = await Restaurant.GetRestaurantAsync(id,restaurantDAL, serviceDAL);
                        List<Meal> meals = await Meal.GetMealsOfOrderAsync(mealDAL,serviceDAL, dishDAL, orderNumber);

                        Order order = new Order(orderNumber, (StatusOrderEnum)status, isDelivery, orderDate, r, lunchService, client, meals);
                        orders.Add(order);
                    }
                }
                return orders;
            }
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
