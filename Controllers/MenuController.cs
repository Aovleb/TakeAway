using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class MenuController : Controller
    {

        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;
        private IMenuDAL menuDAL;
        private IRestaurantDAL restaurantDAL;

        public MenuController(IServiceDAL serviceDAL, IDishDAL dishDAL, IMenuDAL menuDAL, IRestaurantDAL restaurantDAL)
        {
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
            this.menuDAL = menuDAL;
            this.restaurantDAL = restaurantDAL;
        }

        public async Task<IActionResult> Create()
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = GetRestaurantIdInSession();
            if (restaurantId == null)
                return RedirectToAction("UnFound");

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser((int)restaurantId);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            ViewData["RestaurantId"] = restaurantId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = GetRestaurantIdInSession();
            if (restaurantId == null)
                return RedirectToAction("UnFound");

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser((int)restaurantId);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            ViewData["RestaurantId"] = restaurantId;

            if (menu.Dishes == null || menu.Dishes.Count == 0)
            {
                ModelState.AddModelError("Dishes", "At least one dish is required.");
            }

            if (menu.LunchService == null && menu.DinnerService == null)
            {
                ModelState.AddModelError("Services", "At least one service is required.");
            }

            if (ModelState.IsValid)
            {
                //bool success = await menu.CreateAsync(menuDAL, dishDAL, serviceDAL);
                bool success = false;
                if (success)
                {
                    return RedirectToAction("Index", "Restaurant");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create menu.");
                }
                return View(menu);
            }
            return View(menu);
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

        private int? GetRestaurantIdInSession()
        {
            return HttpContext.Session.GetInt32("restaurantId");
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
            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL, (int)userId);
            bool isOwnedByUser = restaurants.Any(r => r.Id == id_restaurant);
            if (!isOwnedByUser)
            {
                return RedirectToAction("UnFound");
            }
            return null;
        }
    }
}
