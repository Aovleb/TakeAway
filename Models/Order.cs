namespace TakeAway.Models
{
    public enum StatusOrderEnum
    {
        InPreparation,
        Ready,
        Delivered,
    }
    public class Order
    {
        private int orderNumber;
        private StatusOrderEnum status;
        private bool isDelivery;
        private DateTime date;
        private Restaurant restaurant;
        private Service service;
        private Client client;
        private List<Meal> meals;

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
        public Restaurant Restaurant
        {
            get { return restaurant; }
            set { restaurant = value; }
        }
        public Service Service
        {
            get { return service; }
            set { service = value; }
        }
        public Client Client
        {
            get { return client; }
            set { client = value; }
        }
        public List<Meal> Meals
        {
            get { return meals; }
            set { meals = value; }
        }
        public Order(int orderNumber, StatusOrderEnum status, bool isDelivery, DateTime date, Restaurant restaurant, Service service, Client client, List<Meal> meals)
        {
            OrderNumber = orderNumber;
            Status = status;
            IsDelivery = isDelivery;
            Date = date;
            Restaurant = restaurant;
            Service = service;
            Client = client;
            Meals = meals;
        }

        public Order() { }

    }
}
