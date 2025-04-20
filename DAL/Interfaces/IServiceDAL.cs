using TakeAway.Models;

namespace TakeAway.DAL.Interfaces
{
    public interface IServiceDAL
    {
        Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Restaurant r);
        Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Dish d);
        Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Menu m);
        Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Order o);
    }
}
