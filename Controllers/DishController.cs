using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

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

        public IActionResult Create()
        {
            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            int? userId = HttpContext.Session.GetInt32("userId");
            if (restaurantId == null || userId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["RestaurantId"] = restaurantId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {
            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            int? userId = HttpContext.Session.GetInt32("userId");
            if (restaurantId == null || userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["RestaurantId"] = restaurantId;
            if (ModelState.IsValid)
            {           
                bool success = await dish.CreateAsync(dishDAL, serviceDAL);
                return RedirectToAction("Index");
            }
            return View(dish);
        }
    }
}
