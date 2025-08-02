using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using TakeAway.Utilities;
using TakeAway.ViewModels;

namespace TakeAway.Controllers
{
    public class BasketController : Controller
    {
        private IMealDAL mealDAL;
        private IOrderDAL orderDAL;
        private IRestaurantDAL restaurantDAL;
        private IUserDAL userDAL;

        public BasketController(IMealDAL mealDAL, IOrderDAL orderDAL, IRestaurantDAL restaurantDAL, IUserDAL userDAL)
        {
            this.mealDAL = mealDAL;
            this.orderDAL = orderDAL;
            this.restaurantDAL = restaurantDAL;
            this.userDAL = userDAL;
        }

        public async Task<IActionResult> Index()
        {
            SetUserViewData();
            //un test
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            int? userId = GetUserIdInSession();
            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            List<MealViewModel> mealDetails = new List<MealViewModel>();

            bool isDeliveryAvailable = false;
            if (basket.RestaurantId.HasValue && userId.HasValue)
            {
                Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, basket.RestaurantId.Value);
                Client client = await Client.GetClientAsync(userDAL, (int)userId);
                double distance = CalculateDistance(restaurant, client);
                isDeliveryAvailable = distance <= 10;
            }
            ViewBag.IsDeliveryAvailable = isDeliveryAvailable;

            foreach ((int id, int quantity) in basket.Items)
            {
                Meal meal = await Meal.GetMealAsync(mealDAL, id);
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
        [ValidateAntiForgeryToken]
        public IActionResult UpdateService(string serviceType)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);

            if (basket.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "The basket is empty !";
                return RedirectToAction("Index");
            }

            basket.ServiceType = serviceType;

            CookieHelper.SetBasketCookie(Response, basket);
            TempData["SuccessMessage"] = "Service update !";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderType(string orderType)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);

            if (basket.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "The basket is empty !";
                return RedirectToAction("Index");
            }

            if (basket.RestaurantId.HasValue && GetUserIdInSession().HasValue)
            {
                Restaurant restaurant = await Restaurant.GetRestaurantAsync(restaurantDAL, basket.RestaurantId.Value);
                Client client = await Client.GetClientAsync(userDAL, (int)GetUserIdInSession());
                double distance = CalculateDistance(restaurant, client);
                if (orderType == "Delivery")
                {
                    if (distance > 10)
                    {
                        TempData["ErrorMessage"] = "Delivery not available for this distance.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        basket.Total += 5;
                        CookieHelper.SetBasketCookie(Response, basket);
                    }
                }
                else
                {
                    if (basket.Total > 5)
                    {
                        basket.Total -= 5;
                        CookieHelper.SetBasketCookie(Response, basket);
                    }
                }
            }

            basket.OrderType = orderType;
            CookieHelper.SetBasketCookie(Response, basket);
            TempData["SuccessMessage"] = "Updated order type !";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Add(int mealId)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            int? restaurantId = HttpContext.Session.GetInt32("restaurantId");
            if (restaurantId == null)
            {
                return UnFound();
            }
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to add to basket.";
                return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            Meal meal = await Meal.GetMealAsync(mealDAL, mealId);
            if (meal != null)
            {
                if (basket.RestaurantId.HasValue && basket.RestaurantId != restaurantId)
                {
                    return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
                }

                basket.Items[mealId] = basket.Items.TryGetValue(mealId, out int qty) ? qty + 1 : 1;
                basket.Total += meal.Price;
                basket.RestaurantId = restaurantId;
                CookieHelper.SetBasketCookie(Response, basket);
                TempData["SuccessMessage"] = "Meal added !";
            }
            return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
        }


        public async Task<IActionResult> Remove(int id)
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                TempData["ErrorMessage"] = "You must be signed in to remove to basket.";
                return checkResult;
            }

            Meal meal = await Meal.GetMealAsync(mealDAL, id);
            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);
            if (basket.Items.ContainsKey(id) && meal != null)
            {
                if (basket.Items[id] > 1) basket.Items[id]--;
                else basket.Items.Remove(id);
                basket.Total -= meal.Price;

                if (!basket.Items.Any())
                {
                    basket.RestaurantId = null;
                }

                CookieHelper.SetBasketCookie(Response, basket);
                TempData["SuccessMessage"] = "Meal removed !";
            }
            return RedirectToAction("Index");
        }


        public IActionResult Clear()
        {
            SetUserViewData();
            int? userId = GetUserIdInSession();
            BasketViewModel basket = new BasketViewModel
            {
                ClientId = userId,
                Items = new Dictionary<int, int>(),
                Total = 0,
                ServiceType = "Lunch",
                RestaurantId = null
            };
            CookieHelper.SetBasketCookie(Response, basket);

            TempData["SuccessMessage"] = "Basket cleared !";
            int? restaurantId = GetRestaurantId();
            if (restaurantId != null)
            {
                return RedirectToAction("Details", "Home", new { id = (int)restaurantId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Pay()
        {
            SetUserViewData();
            IActionResult? checkResult = CheckIsClient();
            if (checkResult != null)
            {
                return checkResult;
            }

            BasketViewModel basket = CookieHelper.GetBasketFromCookie(Request);

            if (basket.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "The basket is empty !";
                return RedirectToAction("Index");
            }

            int? restaurantId = basket.RestaurantId;
            int? clientId = GetUserIdInSession();
            string serviceString = basket.ServiceType;

            Restaurant r = await Restaurant.GetRestaurantAsync(restaurantDAL, (int)restaurantId, true);

            Client c = await Client.GetClientAsync(userDAL, (int)clientId);

            Service s = serviceString == "Lunch" ? r.LunchService : r.DinnerService;

            if (s.StartTime < DateTime.Now.TimeOfDay.Add(new TimeSpan(2, 0, 0)))
            {
                TempData["ErrorMessage"] = "You can only order this service two hours before the service is due to start !";
                return RedirectToAction("Index");
            }

            if (basket.OrderType == "Delivery")
            {
                double distance = CalculateDistance(r, c);
                if (distance > 10)
                {
                    TempData["ErrorMessage"] = "Delivery not available for this distance !";
                    return RedirectToAction("Index");
                }
            }

            bool isDelivery = basket.OrderType == "Delivery";

            Order order = new Order(0, 0, isDelivery, DateTime.Now, s, r, c);

            foreach ((int mealId, int quantity) in basket.Items)
            {
                Meal meal = r.Meals.FirstOrDefault(m => m.Id == mealId);
                if (meal != null && (s.Id == meal.LunchService?.Id || s.Id == meal.DinnerService?.Id))
                {
                    order.AddMeal(meal, quantity);
                }
            }

            bool success = await order.Create(orderDAL);
            if (success)
            {
                TempData["SuccessMessage"] = "Order successfully placed !";
                Dictionary<int, int> items = basket.Items.ToDictionary(x => x.Key, x => x.Value);
                foreach ((int mealId, int quantity) in basket.Items)
                {
                    Meal meal = r.Meals.FirstOrDefault(m => m.Id == mealId);
                    if (meal != null && (s.Id == meal.LunchService?.Id || s.Id == meal.DinnerService?.Id))
                    {
                        items.Remove(mealId);
                    }
                }
                basket.Items = items;
                CookieHelper.SetBasketCookie(Response, basket);

            }
            else
            {
                TempData["ErrorMessage"] = "Order cancelled !";
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


        private int? GetRestaurantId()
        {
            return HttpContext.Session.GetInt32("restaurantId");
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


        private double CalculateDistance(Restaurant restaurant, Client client)
        {
            string restaurantAddress = $"{restaurant.StreetNumber} {restaurant.StreetName}, {restaurant.City}, {restaurant.PostalCode}, {restaurant.Country}";
            string clientAddress = $"{client.StreetNumber} {client.StreetName}, {client.City}, {client.PostalCode}, {client.Country}";

            if (restaurant.PostalCode == client.PostalCode)
                return 5.0;
            return 15.0;
        }
    }
}