using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IClientDAL
    {
        Task<Client> GetOrderAsync(Order o);
        Task<List<Meal>> GetBasketAsync(Client c);
    }
}
