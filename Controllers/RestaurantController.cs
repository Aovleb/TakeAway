using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;

namespace TakeAway.Controllers
{
    public class RestaurantController : Controller
    {
        private IRestaurantDAL restaurantDAL;
        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;
        private IMenuDAL menuDAL;

        public RestaurantController(IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IDishDAL dishDAL, IMenuDAL menuDAL)
        {
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
            this.menuDAL = menuDAL;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = CheckRestaurateurIdInSession();
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL, (int)userId);
            return View(restaurants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurant r)
        {
            int? userId = CheckRestaurateurIdInSession();
            if (ModelState.IsValid)
            {
                bool success = await r.CreateAsync(restaurantDAL, serviceDAL, (int)userId);
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

        public async Task<IActionResult> Dishes(int id)
        {
            CheckRestaurateurIdInSession();
            HttpContext.Session.SetInt32("restaurantId", id);
            List<Dish> dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, id);
            return View(dishes);
        }
        public async Task<IActionResult> Menus(int id)
        {
            CheckRestaurateurIdInSession();
            List<Menu> menus = await Menu.GetRestaurantMenusAsync(menuDAL, dishDAL, serviceDAL, id);
            return View(menus);
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

        private int? CheckRestaurateurIdInSession()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            string? userType = HttpContext.Session.GetString("userType");
            if (userId == null || userType == null || userType != "Restaurateur")
                RedirectToAction("SignIn", "Account");
            return userId;
        }
    }
}
