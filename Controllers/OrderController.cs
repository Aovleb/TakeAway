using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Controllers
{
    public class OrderController : Controller
    {
        private IUserDAL userDAL;
        private IOrderDAL orderDAL;
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;

        public OrderController(IUserDAL userDAL, IOrderDAL orderDAL, IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL)
        {
            this.userDAL = userDAL;
            this.orderDAL = orderDAL;
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
