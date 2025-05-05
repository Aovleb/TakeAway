using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<User> GetUserAsync(string email, string password);
        Task<bool> CreateAsync(User user);
        Task<bool> CreateAsync(RestaurantOwner r);
    }
}
