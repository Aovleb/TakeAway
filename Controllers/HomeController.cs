using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using TakeAway.Utilities;
using TakeAway.ViewModels;

namespace TakeAway.Controllers
{
    public class HomeController : Controller
    {
        private IRestaurantDAL restaurantDAL;

        public HomeController(IRestaurantDAL restaurantDAL)
        {
            this.restaurantDAL = restaurantDAL;
        }

        public async Task<IActionResult> Index()
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsNotRestaurateur();
            if (checkResult != null)
                return checkResult;

            if (GetUserIdInSession() == null)
            {
                BasketViewModel newBasket = new BasketViewModel
                {
                    ClientId = null,
                    Items = new Dictionary<int, int>(),
                    Total = 0,
                    ServiceType = "Lunch",
                    RestaurantId = null
                };
                CookieHelper.SetBasketCookie(Response, newBasket);
            }

            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL);
            return View(restaurants);
        }

        public async Task<IActionResult> Details(int id)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsNotRestaurateur();
            if (checkResult != null)
                return checkResult;

            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, id, true);
            if (restaurant == null)
            {
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetInt32("restaurantId", id);
            return View(restaurant);
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


        private IActionResult? CheckIsNotRestaurateur()
        {
            int? userId = GetUserIdInSession();
            string? userType = GetUserTypeInSession();
            if (userId != null && userType != null && userType == "Restaurateur")
                return RedirectToAction("Logout", "Account");
            return null;
        }
        private void SetUserViewData()
        {
            ViewData["userId"] = HttpContext.Session.GetInt32("userId")?.ToString();
            ViewData["userType"] = HttpContext.Session.GetString("userType");
        }
    }
}
