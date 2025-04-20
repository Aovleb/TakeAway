using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class MenuDAL : IMenuDAL
    {
        private string connectionString;
        public MenuDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Menu>> GetMenusAsync(Restaurant r)
        {
            throw new NotImplementedException();
        }
    }
}
