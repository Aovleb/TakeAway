using System.Collections.Specialized;
using System.ComponentModel;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public enum StatusOrderEnum
    {
        InPreparation = 0,
        Ready = 1,
        Delivered = 2,
    }
    public class Order
    {
        private int orderNumber;
        private StatusOrderEnum status;
        private bool isDelivery;
        private DateTime date;
        private Service service;
        private Restaurant restaurant;
        private Client client;
        private Dictionary<Meal, int> meals;

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }
        public StatusOrderEnum Status
        {
            get { return status; }
            set { status = value; }
        }
        public bool IsDelivery
        {
            get { return isDelivery; }
            set { isDelivery = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public Service Service
        {
            get { return service; }
            set { service = value; }
        }
        public Restaurant Restaurant
        {
            get { return restaurant; }
            set { restaurant = value; }
        }
        public Client Client
        {
            get { return client; }
            set { client = value; }
        }
        public Dictionary<Meal, int> Meals
        {
            get { return meals; }
            set { meals = value; }
        }
        public Order(int orderNumber, StatusOrderEnum status, bool isDelivery, DateTime date, Service service, Restaurant restaurant, Client client)
        {
            OrderNumber = orderNumber;
            Status = status;
            IsDelivery = isDelivery;
            Date = date;
            Service = service;
            Restaurant = restaurant;
            Client = client;
            Meals = new Dictionary<Meal, int>();
        }

        public void AddMeal(Meal meal, int quantity)
        {
            Meals.Add(meal, quantity);
        }

        public Order() { }


        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var meal in Meals)
            {
                totalPrice += meal.Key.Price * meal.Value;
            }
            return totalPrice;
        }

        public static async Task<List<Order>> GetOrdersAsync(Restaurant restaurant,IOrderDAL orderDAL)
        {
            return await orderDAL.GetOrdersAsync(restaurant);
        }

    }
}
