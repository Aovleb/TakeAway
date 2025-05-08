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

            // Récupérer les plats et services disponibles pour le restaurant
            var dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, (int)restaurantId);
            (Service lunchService, Service dinnerService) = await Service.GetRestaurantServicesAsync(serviceDAL, (int)restaurantId);

            ViewData["LunchService"] = lunchService;
            ViewData["DinnerService"] = dinnerService;
            ViewData["Dishes"] = dishes;
            ViewData["RestaurantId"] = restaurantId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu menu, List<int> selectedDishIds, bool chooseLunchService, bool chooseDinnerService)
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
            // Reload dishes and services for view in case of error
            var dishes = await Dish.GetRestaurantDishesAsync(dishDAL, serviceDAL, (int)restaurantId);
            (Service lunchService, Service dinnerService) = await Service.GetRestaurantServicesAsync(serviceDAL, (int)restaurantId);

            ViewData["LunchService"] = lunchService;
            ViewData["DinnerService"] = dinnerService;
            ViewData["Dishes"] = dishes;
            ViewData["SelectedDishIds"] = selectedDishIds; // Passer les plats sélectionnés pour préserver les sélections

            if (selectedDishIds == null || selectedDishIds.Count == 0)
            {
                ModelState.AddModelError("Dishes", "At least one dish is required.");
            }
            else
            {
                // Add selected dishes to the menu
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
                menu.LunchService = chooseLunchService ? lunchService : null;
                menu.DinnerService = chooseDinnerService ? dinnerService : null;
            }

            if (ModelState.IsValid)
            {
                bool success = await menu.CreateAsync(menuDAL, (int)restaurantId);
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