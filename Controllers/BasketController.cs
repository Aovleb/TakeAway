using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;
using TakeAway.DAL;
using TakeAway.ViewModels;

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
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            List<MealViewModel> mealDetails = new List<MealViewModel>();

            foreach ((int id, int quantity) in basket.Items)
            {
                Meal meal = await Meal.GetMealAsync(mealDAL, serviceDAL, dishDAL, id);
                if (meal != null)
                {
                    MealViewModel mealViewModel = new MealViewModel
                    {
                        Id = meal.Id,
                        Name = meal.Name,
                        Price = meal.Price,
                        Type = meal is Menu ? "Menu" : "Dish",
                        LunchService = meal.LunchService,
                        DinnerService = meal.DinnerService
                    };

                    // Si c'est un menu, récupérer les plats associés
                    if (meal is Menu menu)
                    {
                        foreach (Dish dish in menu.Dishes)
                        {
                            mealViewModel.Dishes.Add(new MealViewModel
                            {
                                Id = dish.Id,
                                Name = dish.Name,
                                Price = dish.Price,
                                Type = "Dish",
                                LunchService = dish.LunchService,
                                DinnerService = dish.DinnerService
                            });
                        }
                    }

                    mealDetails.Add(mealViewModel);
                }
            }

            ViewBag.MealDetails = mealDetails;
            return View(basket);
        }

        [HttpPost]
        public IActionResult UpdateService(string serviceType)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            basket.ServiceType = serviceType; // "Lunch" ou "Dinner"

            CookieHelper.SetBasketCookie(Response, basket);
            TempData["Message"] = "Service mis à jour !";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Add(int mealId)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to add to cart.";
                return checkResult;
            }

            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            if (restaurantId == null)
            {
                return UnFound();
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            Meal meal = await Meal.GetMealAsync(mealDAL, serviceDAL, dishDAL, mealId);
            if (meal != null)
            {
                basket.Items[mealId] = basket.Items.TryGetValue(mealId, out int qty) ? qty + 1 : 1;
                basket.Total += meal.Price;
                CookieHelper.SetBasketCookie(Response, basket);
                TempData["SuccessMessage"] = "Article ajouté !";
            }
            return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
        }

        public async Task<IActionResult> Remove(int id)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to add to cart.";
                return checkResult;
            }

            Meal meal = await Meal.GetMealAsync(mealDAL, serviceDAL, dishDAL, id);
            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            if (basket.Items.ContainsKey(id) && meal != null)
            {
                if (basket.Items[id] > 1) basket.Items[id]--;
                else basket.Items.Remove(id);
                basket.Total -= meal.Price;
                CookieHelper.SetBasketCookie(Response, basket);
                TempData["Message"] = "Article retiré !";
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

        private void SetUserViewData()
        {
            ViewData["userId"] = HttpContext.Session.GetInt32("userId")?.ToString();
            ViewData["userType"] = HttpContext.Session.GetString("userType");
        }
    }
}