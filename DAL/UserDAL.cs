using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class UserDAL : IUserDAL
    {
        private string connectionString;
        public UserDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<User> GetUserAsync(string email, string password)
        {
            User u = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Search client table
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM Client c
                                                INNER JOIN person p ON c.id_person = p.id_person
                                                INNER JOIN address a ON c.id_address = a.id_address
                                                WHERE email = @email AND password = @password", conn);
                
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int userId = reader.GetInt32("id_person");
                        string lastname = reader.GetString("lastName");
                        string firstname = reader.GetString("firstName");
                        string phoneNumber = reader.GetString("phoneNumber");
                        string street_name = reader.GetString("street_name");
                        string street_number = reader.GetString("street_number");
                        string postal_code = reader.GetString("postal_code");
                        string city = reader.GetString("city");
                        string country = reader.GetString("country");
                        u = new Client(userId, email, password, lastname, firstname, phoneNumber, street_name, street_number, postal_code, city, country);
                    }
                }

                if (u != null) return u;

                //Search the RestaurantOwner table
                cmd = new SqlCommand(@"SELECT * FROM RestaurantOwner r
                                       INNER JOIN person p ON r.id_person = p.id_person
                                       WHERE email = @email AND password = @password", conn);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int userId = reader.GetInt32("id_person");
                        string name = reader.GetString("name");
                        u = new RestaurantOwner(userId, email, password, name);
                    }
                }

                return u;
            }
        }

        public async Task<bool> CreateAsync(RestaurantOwner r)
        {

            bool success = false;
            string personQuery = @"INSERT INTO person(email,password) OUTPUT INSERTED.id_person VALUES (@email, @password)";
            string restaurantOwnerQuery = @"INSERT INTO restaurantOwner(id_person,name) VALUES (@id_person, @name)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(personQuery, conn, transaction);

                    cmd.Parameters.AddWithValue("@email", r.Email);
                    cmd.Parameters.AddWithValue("@password", r.Password);

                    int personId = (int)await cmd.ExecuteScalarAsync();

                    cmd = new SqlCommand(restaurantOwnerQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_person", personId);
                    cmd.Parameters.AddWithValue("@name", r.Name);

                    int rows = await cmd.ExecuteNonQueryAsync();

                    if (rows > 0)
                    {
                        transaction.Commit();
                        success = true;
                    }
                    else
                        transaction.Rollback();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return success;
        }
        public async Task<bool> CreateAsync(User u)
        {
            throw new Exception("Method not implemented.");
        }
    }
}
