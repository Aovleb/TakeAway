using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IOrderDAL
    {
        Task<List<Order>> GetOrdersAsync(Restaurant r);
    }
}
