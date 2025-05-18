using Microsoft.Data.SqlClient;
using System.Data;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

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


        public async Task<RestaurantOwner> GetRestaurantOwnerAsync(int restaurateurId)
        {
            RestaurantOwner restaurantOwner = null;
            string restaurantOwnerQuery = @"SELECT p.id_person, email, name
                          FROM Person p
                          INNER JOIN RestaurantOwner r ON p.id_person = r.id_person   
                          WHERE p.id_person = @id_person;";

            string restaurantsQuery = @"SELECT r.id_restaurant, r.name, r.description, r.phoneNumber, 
                     a.street_name, a.street_number, a.postal_code, a.city, a.country,
                     s1.id_service AS lunch_service_id, s1.startTime AS lunch_startTime, s1.endTime AS lunch_endTime,
                     s2.id_service AS dinner_service_id, s2.startTime AS dinner_startTime, s2.endTime AS dinner_endTime
                      FROM Restaurant r
                      INNER JOIN Address a ON a.id_address = r.id_address
                      LEFT JOIN Service s1 ON s1.id_restaurant = r.id_restaurant AND s1.startTime = (SELECT MIN(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
                      LEFT JOIN Service s2 ON s2.id_restaurant = r.id_restaurant AND s2.startTime = (SELECT MAX(startTime) FROM Service WHERE id_restaurant = r.id_restaurant)
                      WHERE r.id_person = @id_person";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand cmdOwner = new SqlCommand(restaurantOwnerQuery,conn);
                cmdOwner.Parameters.AddWithValue("@id_person", restaurateurId);

                using (SqlDataReader readerOwner = await cmdOwner.ExecuteReaderAsync())
                {
                    if (await readerOwner.ReadAsync())
                    {
                        int id = readerOwner.GetInt32("id_person");
                        string email = readerOwner.GetString("email");
                        string name = readerOwner.GetString("name");

                        restaurantOwner = new RestaurantOwner(id, email, null, name);
                    }
                    else
                    {
                        return null;
                    }
                }

                SqlCommand cmdRestaurants = new SqlCommand(restaurantsQuery, conn);
                cmdRestaurants.Parameters.AddWithValue("@id_person", restaurateurId);

                using (SqlDataReader reader = await cmdRestaurants.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int restaurantId = reader.GetInt32("id_restaurant");
                        string name = reader.GetString("name");
                        string description = reader.GetString("description");
                        string phoneNumber = reader.GetString("phoneNumber");
                        string streetName = reader.GetString("street_name");
                        string streetNumber = reader.GetString("street_number");
                        string postalCode = reader.GetString("postal_code");
                        string city = reader.GetString("city");
                        string country = reader.GetString("country");

                        int lunchServiceId = reader.GetInt32("lunch_service_id");
                        TimeSpan lunchStartTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_startTime"));
                        TimeSpan lunchEndTime = reader.GetTimeSpan(reader.GetOrdinal("lunch_endTime"));
                        Service lunchService = new Service(lunchServiceId, lunchStartTime, lunchEndTime);
                        
                        int dinnerServiceId = reader.GetInt32("dinner_service_id");
                        TimeSpan dinnerStartTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_startTime"));
                        TimeSpan dinnerEndTime = reader.GetTimeSpan(reader.GetOrdinal("dinner_endTime"));
                        Service dinnerService = new Service(dinnerServiceId, dinnerStartTime, dinnerEndTime);

                        Restaurant r = new Restaurant(restaurantId, name, description, phoneNumber, streetName, streetNumber, postalCode, city, country, lunchService, dinnerService);
                        restaurantOwner.AddRestaurant(r);
                    }
                }
            }
            return restaurantOwner;
        }


        public async Task<bool> CreateAsync(RestaurantOwner r)
        {
            bool success = false;
            string checkEmailQuery = @"SELECT COUNT(*) FROM person WHERE email = @email";
            string personQuery = @"INSERT INTO person(email,password) OUTPUT INSERTED.id_person VALUES (@email, @password)";
            string restaurantOwnerQuery = @"INSERT INTO restaurantOwner(id_person,name) VALUES (@id_person, @name)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, conn);
                checkEmailCmd.Parameters.AddWithValue("@email", r.Email);

                int emailCount = (int)await checkEmailCmd.ExecuteScalarAsync();

                if (emailCount > 0)
                {
                    return false;
                }

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


        public async Task<bool> CreateAsync(Client c)
        {
            bool success = false;
            string checkEmailQuery = @"SELECT COUNT(*) FROM person WHERE email = @email";
            string personQuery = @"INSERT INTO person(email,password) OUTPUT INSERTED.id_person 
                                               VALUES(@email, @password)";
            string addressQuery = @"INSERT INTO address(street_name, street_number, postal_code, city, country) OUTPUT INSERTED.id_address
                                                VALUES(@street_name, @street_number, @postal_code, @city, @country)";
            string clientQuery = @"INSERT INTO client(id_person, id_address, lastName, firstName, phoneNumber) 
                                               VALUES(@id_person, @id_address, @lastName, @firstName, @phoneNumber)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, conn);
                checkEmailCmd.Parameters.AddWithValue("@email", c.Email);

                int emailCount = (int)await checkEmailCmd.ExecuteScalarAsync();

                if (emailCount > 0)
                {
                    return false;
                }

                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(personQuery, conn, transaction);

                    cmd.Parameters.AddWithValue("@email", c.Email);
                    cmd.Parameters.AddWithValue("@password", c.Password);

                    int personId = (int)await cmd.ExecuteScalarAsync();

                    cmd = new SqlCommand(addressQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@street_name", c.StreetName);
                    cmd.Parameters.AddWithValue("@street_number", c.StreetNumber);
                    cmd.Parameters.AddWithValue("@postal_code", c.PostalCode);
                    cmd.Parameters.AddWithValue("@city", c.City);
                    cmd.Parameters.AddWithValue("@country", c.Country);

                    int addressId = (int)await cmd.ExecuteScalarAsync();

                    cmd = new SqlCommand(clientQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@id_person", personId);
                    cmd.Parameters.AddWithValue("@id_address", addressId);
                    cmd.Parameters.AddWithValue("@lastName", c.LastName);
                    cmd.Parameters.AddWithValue("@firstName", c.FirstName);
                    cmd.Parameters.AddWithValue("@phoneNumber", c.PhoneNumber);

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


        public async Task<Client> GetClientAsync(int userId)
        {
            Client client = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Client c
                                 INNER JOIN person p ON c.id_person = p.id_person
                                 INNER JOIN address a ON c.id_address = a.id_address
                                 WHERE c.id_person = @userId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
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
                        client = new Client(userId, email, password, lastname, firstname, phoneNumber, street_name, street_number, postal_code, city, country);
                    }
                }
            }
            return client;
        }
    }
}
