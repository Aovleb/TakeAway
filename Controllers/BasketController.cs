using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;

namespace TakeAway.Controllers
{
    public class BasketController : Controller
    {

        private IClientDAL clientDAL;
        private IServiceDAL serviceDAL;
        private IDishDAL dishDAL;
        private IMealDAL mealDAL;

        public BasketController(IClientDAL clientDAL, IServiceDAL serviceDAL, IDishDAL dishDAL, IMealDAL mealDAL)
        {
            this.clientDAL = clientDAL;
            this.serviceDAL = serviceDAL;
            this.dishDAL = dishDAL;
            this.mealDAL = mealDAL;
        }


        public async Task<IActionResult> Index()
        {
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            int? userId = GetUserIdInSession();

            List<Meal> meals = await Client.GetBasketAsync(clientDAL, serviceDAL, dishDAL, (int)userId);

            return View(meals);
        }

        public async Task<IActionResult> Add(int mealId)
        {
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to add to cart.";
                return checkResult;
            }

            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            if(restaurantId == null)
            {
                return UnFound();
            }
            int? userId = GetUserIdInSession();

            Meal m = await Meal.GetMealAsync(mealDAL, serviceDAL, dishDAL, mealId);

            bool success = m != null && await m.AddInBasket(mealDAL, (int)userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Item successfully added to the cart!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add item to the cart. Please try again.";
            }
            return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
        }

        public async Task<IActionResult> Remove(int mealId)
        {
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to add to cart.";
                return checkResult;
            }

            int? userId = GetUserIdInSession();

            Meal m = await Meal.GetMealAsync(mealDAL, serviceDAL, dishDAL, mealId);

            bool success = m != null && await m.RemoveFromBasket(mealDAL, (int)userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Item successfully remove to the basket!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to remove item to the cart. Please try again.";
            }
            return RedirectToAction("Index");
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

        private IActionResult? CheckIsClient()
        {
            int? userId = GetUserIdInSession();
            string? userType = GetUserTypeInSession();
            if (userId == null || userType == null || userType != "Client")
                return RedirectToAction("UnFound");
            return null;
        }
    }
}
