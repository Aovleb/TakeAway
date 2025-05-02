using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IServiceDAL
    {
        Task<(Service lunchService, Service dinnerService)> GetRestaurantServicesAsync(int id);
        Task<(Service lunchService, Service dinnerService)> GetOrderServicesAsync(int id);
        Task<(Service lunchService, Service dinnerService)> GetMealServicesAsync(int id);
    }
}
