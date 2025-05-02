using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

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
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL);
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
