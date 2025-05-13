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

        public async Task<Client> GetOrderAsync(Order o)
        {
            throw new NotImplementedException();
        }
    }
}
