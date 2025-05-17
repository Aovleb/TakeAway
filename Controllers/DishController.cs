using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class DishController : Controller
    {
        private IMealDAL mealDAL;
        private IRestaurantDAL restaurantDAL;
        private IUserDAL userDAL;

        public DishController(IMealDAL mealDAL, IRestaurantDAL restaurantDAL, IUserDAL userDAL)
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

            ViewData["RestaurantId"] = restaurantId;

            Restaurant r = await Restaurant.GetRestaurantAsync(restaurantDAL, (int)restaurantId);

            ViewData["LunchService"] = r.LunchService;
            ViewData["DinnerService"] = r.DinnerService;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish dish, bool chooseLunchService, bool chooseDinnerService)
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


            Restaurant r = await Restaurant.GetRestaurantAsync(restaurantDAL, (int)restaurantId);

            dish.LunchService = chooseLunchService ? r.LunchService : null;
            dish.DinnerService = chooseDinnerService ? r.DinnerService : null;

            ViewData["RestaurantId"] = restaurantId;
            ViewData["LunchService"] = r.LunchService;
            ViewData["DinnerService"] = r.DinnerService;

            //// Créer un nouveau ModelState pour limiter la validation aux champs souhaités
            //var validFields = new[] { "Name", "Description", "Price" };
            //var newModelState = new ModelStateDictionary();
            //foreach (var field in validFields)
            //{
            //    var modelStateEntry = ModelState[field];
            //    if (modelStateEntry != null)
            //    {
            //        newModelState.AddModelState(field, modelStateEntry);
            //    }
            //}
            //ModelState.Clear();
            //ModelState.Merge(newModelState);

            ModelState.Remove("LunchService");
            ModelState.Remove("DinnerService");
            ModelState.Remove("LunchService.StartTime");
            ModelState.Remove("LunchService.EndTime");
            ModelState.Remove("DinnerService.StartTime");
            ModelState.Remove("DinnerService.EndTime");

            if (ModelState.IsValid)
            {
                bool success = await dish.CreateAsync(mealDAL, (int)restaurantId);
                if (success)
                {
                    return RedirectToAction("Dishes", "Restaurant", new { id = restaurantId });
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