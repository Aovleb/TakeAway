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
            HttpContext.Session.SetString("UserType", "User");
            HttpContext.Session.SetInt32("UserId", 1);
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsForClientAsync(restaurantDAL, serviceDAL);
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int id)
        {
            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, serviceDAL, id);
            if (restaurant == null)
            {
                return NotFound();
            }
            List<Meal> meals = await Meal.GetRestaurantMealsAsync(mealDAL, menuDAL, dishDAL, serviceDAL, id);
            meals.ForEach(m => restaurant.AddMeal(m));
            return View(restaurant);
        }
    }
}
