using Microsoft.AspNetCore.Mvc;
using TakeAway.Models;

namespace TakeAway.BL.Interfaces
{
    public interface IBasketBL
    {
        double CalculateDistance(Restaurant restaurant, Client client);
    }
}
