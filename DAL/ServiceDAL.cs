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

        public async Task<(Service lunchService, Service dinnerService)> GetRestaurantServicesAsync(int id)
        {
            Service lunchService = null;
            Service dinnerService = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Service s WHERE id_restaurant = @id ORDER BY startTime", conn);
                cmd.Parameters.AddWithValue("@id", id);
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
                        s.StartTime = startTimeSpan;
                        s.EndTime = endTimeSpan;
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

        public async Task<(Service lunchService, Service dinnerService)> GetMealServicesAsync(int id)
        {
            Service lunchService = null;
            Service dinnerService = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Service s INNER JOIN Meal_Service ms ON s.id_service = ms.id_service AND ms.id_meal = @id ORDER BY startTime", conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    int cpt = 0;
                    while (await reader.ReadAsync())
                    {
                        int service_id = reader.GetInt32("id_service");
                        TimeSpan startTimeSpan = reader.GetTimeSpan(reader.GetOrdinal("startTime"));
                        TimeSpan endTimeSpan = reader.GetTimeSpan(reader.GetOrdinal("endTime"));
                        Service s = new Service(service_id, startTimeSpan, endTimeSpan);
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

        public async Task<(Service lunchService, Service dinnerService)> GetOrderServicesAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
