using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public class Service
    {
        private int id;
        private DateTime startTime;
        private DateTime endTime;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public Service(int id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }
        public Service() { }

        public async static Task<(Service lunchService, Service dinnerService)> GetServicesAsync(IServiceDAL serviceDAL, Restaurant r)
        {
            return await serviceDAL.GetServicesAsync(r);
        }
    }
}
