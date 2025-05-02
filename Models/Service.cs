using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Service
    {
        private int id;
        private TimeSpan startTime;
        private TimeSpan endTime;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public Service(int id, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }
        public Service() { }

        public async static Task<(Service lunchService, Service dinnerService)> GetRestaurantServicesAsync(IServiceDAL serviceDAL, int id)
        {
            return await serviceDAL.GetRestaurantServicesAsync(id);
        }

        public async static Task<(Service lunchService, Service dinnerService)> GetMealServicesAsync(IServiceDAL serviceDAL, int id)
        {
            return await serviceDAL.GetMealServicesAsync(id);
        }
    }
}
