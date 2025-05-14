using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class ClientDAL : IClientDAL
    {
        private string connectionString;
        public ClientDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<Client> GetOrderAsync(int id)
        {
            Client c = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM client c 
                                                  INNER JOIN Address a ON c.id_address = a.id_address
                                                  INNER JOIN ClientOrder o ON c.id_person = o.id_person 
                                                  INNER JOIN Person p ON c.id_person = p.id_person
                                                  WHERE o.orderNumber = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int userId = reader.GetInt32("id_person");
                        string email = reader.GetString("email");
                        string password = reader.GetString("password");
                        string lastname = reader.GetString("lastName");
                        string firstname = reader.GetString("firstName");
                        string phoneNumber = reader.GetString("phoneNumber");
                        string street_name = reader.GetString("street_name");
                        string street_number = reader.GetString("street_number");
                        string postal_code = reader.GetString("postal_code");
                        string city = reader.GetString("city");
                        string country = reader.GetString("country");
                        c = new Client(userId, email, password, lastname, firstname, phoneNumber, street_name, street_number, postal_code, city, country);
                    }
                }
                return c;
            }
        }
    }
}
