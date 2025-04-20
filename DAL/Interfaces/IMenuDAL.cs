using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IMenuDAL
    {
        Task<List<Menu>> GetMenusAsync(Restaurant r);
    }
}
