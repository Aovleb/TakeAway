using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.DAL
{
    public class MealDAL : IMealDAL
    {
        private string connectionString;
        public MealDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Meal>> GetMealsAsync(Restaurant r)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Meal>> GetMealsAsync(Order o)
        {
            throw new NotImplementedException();
        }
    }
}
