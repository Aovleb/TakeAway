using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IOrderDAL
    {
        Task<List<Order>> GetOrdersAsync(Restaurant restaurant);
        Task<Order?> GetOrderAsync(int orderNumber);
        Task<bool> UpdateOrderStatusAsync(int orderNumber, StatusOrderEnum status);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> Create(Order order);
    }
}
