using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;

namespace TakeAway.Controllers
{
    public class RestaurantController : Controller
    {
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;
        private IMealDAL mealDAL;
        private IMenuDAL menuDAL;
        private IDishDAL dishDAL;

        public RestaurantController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IMealDAL mealDAL, IMenuDAL menuDAL, IDishDAL dishDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.mealDAL = mealDAL;
            this.menuDAL = menuDAL;
            this.dishDAL = dishDAL;

        }
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("UserType", "Restaurateur");
            HttpContext.Session.SetInt32("UserId", 1);
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(HttpContext.Session.GetString("UserType") != "Restaurateur")
            {
                return RedirectToAction("Index", "Home");
            }
            
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            List<Restaurant> restaurants = await Restaurant.GetRestaurantsForRestaurateurAsync(restaurantDAL, serviceDAL, userId);
            return View(restaurants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurant r)
        {
            
            if (ModelState.IsValid && HttpContext.Session.GetInt32("UserId") != null)
            {
                int userId = HttpContext.Session.GetInt32("UserId").Value;
                bool success = await r.CreateAsync(restaurantDAL, serviceDAL, userId);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(r);
                }
            }
            return View(r);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
