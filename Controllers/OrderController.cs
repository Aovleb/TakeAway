using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

namespace TakeAway.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderDAL orderDAL;
        private readonly IRestaurantDAL restaurantDAL;
        private readonly IUserDAL userDAL;

        public OrderController(IOrderDAL orderDAL, IRestaurantDAL restaurantDAL, IUserDAL userDAL)
        {
            this.orderDAL = orderDAL;
            this.restaurantDAL = restaurantDAL;
            this.userDAL = userDAL;
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

            Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, id);

            List<Order> orders = await Order.GetOrdersAsync(restaurant, orderDAL);

            HttpContext.Session.SetInt32("restaurantId", id);

            return View(orders);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ErrorMessage"] = "You are not authorized to modify this order.";
                return checkOwnedByUser;
            }

            Order order = new Order() { OrderNumber = orderNumber };

            bool updated = await order.UpdateOrderStatusAsync(orderDAL, status);
            if (!updated)
            {
                TempData["ErrorMessage"] = "Status update failed.";
                return RedirectToAction("Index", new { id = (int)restaurantId });
            }

            TempData["SuccessMessage"] = "Order status updated.";
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