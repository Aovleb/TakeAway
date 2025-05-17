using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Models
{
    public enum StatusOrderEnum
    {
        [Display(Name ="In Preparation")]
        InPreparation = 0,
        [Display(Name = "Ready for Delivery")]
        Ready = 1,
        [Display(Name = "Delivered")]
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

        public Order() 
        {
            Meals = new Dictionary<Meal, int>();
        }

        public void AddMeal(Meal meal, int quantity)
        {
            Meals.Add(meal, quantity);
        }

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var meal in Meals)
            {
                totalPrice += meal.Key.Price * meal.Value;
            }
            if (IsDelivery)
            {
                totalPrice += 5;
            }
            return totalPrice;
        }

        public static async Task<List<Order>> GetOrdersAsync(Restaurant restaurant,IOrderDAL orderDAL)
        {
            return await orderDAL.GetOrdersAsync(restaurant);
        }

        public async Task<bool> UpdateOrderStatusAsync(IOrderDAL orderDAL, StatusOrderEnum status)
        {
            return await orderDAL.UpdateOrderStatusAsync(this.OrderNumber, status);
        }

        public async Task<bool> Create(IOrderDAL orderDAL)
        {
            return await orderDAL.Create(this);
        }
    }
}
