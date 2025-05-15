using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class OrderController : Controller
    {
        private readonly IClientDAL clientDAL;
        private readonly IOrderDAL orderDAL;
        private readonly IRestaurantDAL restaurantDAL;
        private readonly IServiceDAL serviceDAL;
        private readonly IMealDAL mealDAL;
        private readonly IDishDAL dishDAL;

        public OrderController(IClientDAL clientDAL, IOrderDAL orderDAL, IRestaurantDAL restaurantDAL, IServiceDAL serviceDAL, IMealDAL mealDAL, IDishDAL dishDAL)
        {
            this.clientDAL = clientDAL;
            this.orderDAL = orderDAL;
            this.restaurantDAL = restaurantDAL;
            this.serviceDAL = serviceDAL;
            this.mealDAL = mealDAL;
            this.dishDAL = dishDAL;
        }

        public async Task<IActionResult> Index(int id)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser(id);
            if (checkOwnedByUser != null)
                return checkOwnedByUser;

            List<Order> orders = await Order.GetOrdersAsync(id, orderDAL, clientDAL, serviceDAL, mealDAL, restaurantDAL, dishDAL);

            HttpContext.Session.SetInt32("restaurantId", id); // Stocker l'ID du restaurant pour les redirections

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderNumber, StatusOrderEnum status)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsRestaurateur();
            if (checkResult != null)
                return checkResult;

            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            IActionResult? checkOwnedByUser = await CheckIsOwnedByUser((int)restaurantId);
            if (checkOwnedByUser != null)
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas autorisé à modifier cette commande.";
                return checkOwnedByUser;
            }

            // Mettre à jour le statut dans la base de données
            bool updated = await orderDAL.UpdateOrderStatusAsync(orderNumber, status);
            if (!updated)
            {
                TempData["ErrorMessage"] = "Échec de la mise à jour du statut.";
                return RedirectToAction("Index", new { id = (int)restaurantId });
            }

            TempData["SuccessMessage"] = "Statut de la commande mis à jour.";
            return RedirectToAction("Index", new { id = (int)restaurantId });
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
            if (userId == null)
                return RedirectToAction("UnFound");

            List<Restaurant> restaurants = await Restaurant.GetRestaurantsAsync(restaurantDAL, serviceDAL, (int)userId);
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