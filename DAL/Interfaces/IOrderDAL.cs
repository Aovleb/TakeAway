using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IOrderDAL
    {
        Task<List<Order>> GetOrdersAsync(int id, IClientDAL clientDAL, IServiceDAL serviceDAL, IMealDAL mealDAL, IRestaurantDAL restaurantDAL, IDishDAL dishDAL);
        Task<bool> UpdateOrderStatusAsync(int orderNumber, StatusOrderEnum status);
    }
}
