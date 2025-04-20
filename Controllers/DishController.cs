using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Controllers
{
    public class DishController : Controller
    {
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;

        public DishController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IDishDAL dishDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
