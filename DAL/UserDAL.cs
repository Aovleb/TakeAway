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
            throw new NotImplementedException();
        }
    }
}
