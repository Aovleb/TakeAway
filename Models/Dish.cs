namespace TakeAway.Models
{
    public class Dish : Meal
    {
        public Dish(int id, string name, string description, decimal price, Service lunchService, Service dinnerService) : base(id, name, description, price, lunchService, dinnerService) { }
    }
}
