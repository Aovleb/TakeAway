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
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? userId = GetUserIdInSession();
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL, (int)userId);
            return View(restaurants);
        }

        public IActionResult Create()
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurant r)
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? userId = GetUserIdInSession();

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
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            HttpContext.Session.SetInt32("restaurantId", id);
            List<Dish> dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, id);
            return View(dishes);
        }

        public async Task<IActionResult> Menus(int id)
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            HttpContext.Session.SetInt32("restaurantId", id);
            List<Menu> menus = await Menu.GetRestaurantMenusAsync(menuDAL, dishDAL, serviceDAL, id);
            return View(menus);
        }

        public IActionResult UnFound()
        {
            return RedirectToAction("SignIn", "Account");
        }

        private int? GetUserIdInSession()
        {
            return HttpContext.Session.GetInt32("userId");
        }

        private string? GetUserTypeInSession()
        {
            return HttpContext.Session.GetString("userType");
        }

        private IActionResult? CheckIsRestaurateur()
        {
            int? userId = GetUserIdInSession();
            string? userType = GetUserTypeInSession();
            if (userId == null || userType == null || userType != "Restaurateur")
                return RedirectToAction("UnFound");
            return null;
        }
    }
}
