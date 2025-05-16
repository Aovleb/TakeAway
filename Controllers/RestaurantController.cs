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

        public RestaurantController(IRestaurantDAL restaurantDAL)
        {
            this.restaurantDAL = restaurantDAL;
        }

        public async Task<IActionResult> Index()
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            HttpContext.Session.Remove("restaurantId");

            int? userId = GetUserIdInSession();
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, (int)userId);
            return View(restaurants);
        }

        public IActionResult Create()
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant r)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? userId = GetUserIdInSession();

            if (ModelState.IsValid)
            {
                bool success = await r.CreateAsync(restaurantDAL, (int)userId);
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
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser(id);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            HttpContext.Session.SetInt32("restaurantId", id);

            
            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, id, true);
            List<Dish> dishes = new List<Dish>();
            restaurant.Meals.ForEach(m =>
            {
                if (m is Dish dish)
                {
                    dishes.Add(dish);
                }
            });
            return View(dishes);
        }

        public async Task<IActionResult> Menus(int id)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser(id);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            HttpContext.Session.SetInt32("restaurantId", id);

            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, id, true);
            List<Menu> menus = new List<Menu>();
            restaurant.Meals.ForEach(m =>
            {
                if (m is Menu menu)
                {
                    menus.Add(menu);
                }
            });
            return View(menus);
        }

        public IActionResult UnFound()
        {
            return RedirectToAction("Logout", "Account");
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

        private async Task<IActionResult?> CheckIsOwnedByUser(int id_restaurant)
        {
            int? userId = GetUserIdInSession();
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, (int)userId);
            bool isOwnedByUser = restaurants.Any(r => r.Id == id_restaurant);
            if (!isOwnedByUser)
            {
                return RedirectToAction("UnFound");
            }
            return null;
        }

        private void SetUserViewData()
        {
            ViewData["userId"] = HttpContext.Session.GetInt32("userId")?.ToString();
            ViewData["userType"] = HttpContext.Session.GetString("userType");
        }
    }
}
