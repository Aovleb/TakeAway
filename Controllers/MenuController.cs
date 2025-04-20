using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;

namespace TakeAway.Controllers
{
    public class MenuController : Controller
    {
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;
        private IMenuDAL menuDAL;

        public MenuController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IDishDAL dishDAL, IMenuDAL menuDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
            this.menuDAL = menuDAL;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
