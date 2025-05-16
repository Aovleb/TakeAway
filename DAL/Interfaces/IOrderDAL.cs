using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IOrderDAL
    {
        Task<List<Order>> GetOrdersAsync(Restaurant restaurant);
        Task<bool> UpdateOrderStatusAsync(int orderNumber, StatusOrderEnum status);
    }
}
