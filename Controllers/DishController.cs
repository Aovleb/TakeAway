using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class DishController : Controller
    {
        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;

        public DishController(IServiceDAL serviceDAL, IDishDAL dishDAL)
        {
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
        }

        public IActionResult Create()
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = GetRestaurantIdInSession();
            if (restaurantId == null)
                return RedirectToAction("UnFound");

            ViewData["RestaurantId"] = restaurantId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = GetRestaurantIdInSession();
            if (restaurantId == null)
                return RedirectToAction("UnFound");

            ViewData["RestaurantId"] = restaurantId;

            if (dish.LunchService == null && dish.DinnerService == null)
            {
                ModelState.AddModelError("Services", "At least one service is required.");
            }

            if (ModelState.IsValid)
            {
                bool success = await dish.CreateAsync(dishDAL, serviceDAL);
                if (success)
                {
                    return RedirectToAction("Index", "Restaurant");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create dish.");
                }
                return View(dish);
            }
            return View(dish);
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
    }
}