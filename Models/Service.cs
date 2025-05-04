using System.ComponentModel.DataAnnotations;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Service
    {
        private int id;
        private TimeSpan? startTime;
        private TimeSpan? endTime;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan? StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeSpan? EndTime
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
