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

        public RestaurantController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
        }

        public async Task<IActionResult> Index()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            return View(restaurants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurant r)
        {
            
            if (ModelState.IsValid)
            {
                bool success = await r.CreateAsync(restaurantDAL, serviceDAL, 1);
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
