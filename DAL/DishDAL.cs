using Microsoft.Data.SqlClient;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using System.Data;
using System.Data.SqlTypes;

namespace TakeAway.DAL
{
    public class DishDAL : IDishDAL
    {
        private string connectionString;
        public DishDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Dish>> GetDishesAsync(Restaurant r)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Dish>> GetDishesAsync(Menu m)
        {
            throw new NotImplementedException();
        }
    }
}
