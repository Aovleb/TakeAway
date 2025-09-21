using TakeAway.BL.Interfaces;
using TakeAway.Models;

namespace TakeAway.BL
{
    public class BasketBL : IBasketBL
    {
        public double CalculateDistance(Restaurant restaurant, Client client)
        {
            string restaurantAddress = $"{restaurant.StreetNumber} {restaurant.StreetName}, {restaurant.City}, {restaurant.PostalCode}, {restaurant.Country}";
            string clientAddress = $"{client.StreetNumber} {client.StreetName}, {client.City}, {client.PostalCode}, {client.Country}";

            if (restaurant.PostalCode == client.PostalCode)
                return 5.0;
            return 15.0;
        }
    }
}
