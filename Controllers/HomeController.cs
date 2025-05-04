using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;

namespace TakeAway.Controllers
{
    public class HomeController : Controller
    {
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;
        private IMealDAL mealDAL;
        private IMenuDAL menuDAL;
        private IDishDAL dishDAL;

        public HomeController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.mealDAL = mealDAL;
            this.menuDAL = menuDAL;
            this.dishDAL = dishDAL;
        }

        public async Task<IActionResult> Index()
        {
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL);
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int id)
        {
            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, serviceDAL, mealDAL, menuDAL, dishDAL, id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }
    }
}
