using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IClientDAL
    {
        Task<Client> GetOrderAsync(Order o);
    }
}
