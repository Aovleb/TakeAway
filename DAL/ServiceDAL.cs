using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class ServiceDAL : IServiceDAL
    {
        private string connectionString;
        public ServiceDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Restaurant r)
        {
            Service lunchService = null;
            Service dinnerService = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Service s WHERE id_restaurant = @id ORDER BY startTime", conn);
                cmd.Parameters.AddWithValue("@id", r.Id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int cpt = 0;
                    while (await reader.ReadAsync())
                    {
                        Service s = new Service();
                        s.Id = reader.GetInt32("id_service");
                        TimeSpan startTimeSpan = reader.GetTimeSpan(reader.GetOrdinal("startTime"));
                        TimeSpan endTimeSpan = reader.GetTimeSpan(reader.GetOrdinal("endTime"));
                        s.StartTime = new DateTime(1, 1, 1, startTimeSpan.Hours, startTimeSpan.Minutes, startTimeSpan.Seconds);
                        s.EndTime = new DateTime(1, 1, 1, endTimeSpan.Hours, endTimeSpan.Minutes, endTimeSpan.Seconds);
                        if (cpt == 0)
                            lunchService = s;
                        else
                            dinnerService = s;
                        cpt++;
                    }
                }
                return (lunchService, dinnerService);
            }
        }

        public async Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Dish d)
        {
            throw new NotImplementedException();
        }

        public async Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Menu m)
        {
            throw new NotImplementedException();
        }

        public async Task<(Service lunchService, Service dinnerService)> GetServicesAsync(Order o)
        {
            throw new NotImplementedException();
        }
    }
}
