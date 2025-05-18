using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class MenuController : Controller
    {
        private IMealDAL mealDAL;
        private IRestaurantDAL restaurantDAL;
        private IUserDAL userDAL;

        public MenuController(IMealDAL mealDAL, IRestaurantDAL restaurantDAL, IUserDAL userDAL)
        {
            this.mealDAL = mealDAL;
            this.restaurantDAL = restaurantDAL;
            this.userDAL = userDAL;
        }

        public async Task<IActionResult> Create()
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = GetRestaurantIdInSession();
            if (restaurantId == null)
                return RedirectToAction("UnFound");

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser((int)restaurantId);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            Restaurant r = await Restaurant.GetRestaurantAsync(restaurantDAL, (int)restaurantId,true);
            List<Dish> dishes = new();
            r.Meals.ForEach(m =>
            {
                if (m is Dish)
                {
                    Dish dish = (Dish)m;
                    dishes.Add(dish);
                }
            });


            ViewData["LunchService"] = r.LunchService;
            ViewData["DinnerService"] = r.DinnerService;
            ViewData["Dishes"] = dishes;
            ViewData["RestaurantId"] = restaurantId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu, List<int> selectedDishIds, bool chooseLunchService, bool chooseDinnerService)
        {
            SetUserViewData();
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
            Restaurant r = await Restaurant.GetRestaurantAsync(restaurantDAL, (int)restaurantId, true);
            List<Dish> dishes = new();
            r.Meals.ForEach(m =>
            {
                if (m is Dish)
                {
                    Dish dish = (Dish)m;
                    dishes.Add(dish);
                }
            });
            ViewData["LunchService"] = r.LunchService;
            ViewData["DinnerService"] = r.DinnerService;
            ViewData["Dishes"] = dishes;
            ViewData["SelectedDishIds"] = selectedDishIds;

            if (selectedDishIds == null || selectedDishIds.Count == 0)
            {
                ModelState.AddModelError("Dishes", "At least one dish is required.");
            }
            else
            {
                dishes.ForEach(dish =>
                {
                    if (selectedDishIds.Contains(dish.Id))
                    {
                        menu.AddDish(dish);
                    }
                });
            }

            if (!chooseLunchService && !chooseDinnerService)
            {
                ModelState.AddModelError("LunchService", "At least one service is required.");
            }
            else
            {
                menu.LunchService = chooseLunchService ? r.LunchService : null;
                menu.DinnerService = chooseDinnerService ? r.DinnerService : null;
            }

            if (ModelState.IsValid)
            {
                bool success = await menu.CreateAsync(mealDAL, (int)restaurantId);
                if (success)
                {
                    return RedirectToAction("Menus", "Restaurant", new { id = restaurantId });
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
            RestaurantOwner restaurantOwner = await RestaurantOwner.GetRestaurantOwnerAsync(userDAL, (int)userId);
            bool isOwnedByUser = restaurantOwner.Restaurants.Any(r => r.Id == id_restaurant);
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