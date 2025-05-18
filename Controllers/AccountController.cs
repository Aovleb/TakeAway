using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;
using TakeAway.ViewModels;
using System.Reflection;
namespace TakeAway.Controllers
{
    public class AccountController : Controller
    {
        private IUserDAL userDAL;
        public AccountController(IUserDAL userDAL)
        {
            this.userDAL = userDAL;
        }
        
        public IActionResult SignIn()
        {
            SetUserViewData();
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            SetUserViewData();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await TakeAway.Models.User.GetUserAsync(userDAL, model.Email, model.Password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("userId", user.Id);

                BasketViewModel newBasket = new BasketViewModel
                {
                    ClientId = user.Id,
                    Items = new Dictionary<int, int>(),
                    Total = 0,
                    ServiceType = "Lunch",
                    RestaurantId = null
                };
                CookieHelper.SetBasketCookie(Response, newBasket);

                if (user is Client)
                {
                    HttpContext.Session.SetString("userType", "Client");
                    return RedirectToAction("Index", "Home");
                }
                else if (user is RestaurantOwner)
                {
                    HttpContext.Session.SetString("userType", "Restaurateur");
                    return RedirectToAction("Index", "Restaurant");
                }
            }

            ViewData["ErrorMessage"] = "Invalid email address or password.";
            return View();
        }

        public IActionResult SignUp()
        {
            SetUserViewData();
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return RedirectToAction("Index", "Home");
            }


            ViewData["ActiveForm"] = "Client";
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterClient(SignUpViewModel model)
        {
            SetUserViewData();
            ViewData["ActiveForm"] = "Client";
            bool successForm = true;

            if (!model.IsValid(ModelState, model.Client?.Password))
            {
                successForm = false;
            }

            if (model.Client == null || !ModelState.IsValid)
            {
                successForm = false;
            }

            if (!successForm)
            {
                model.Client ??= new Client();
                model.RestaurantOwner ??= new RestaurantOwner();
                return View("SignUp", model);
            }

            bool success = await model.Client.CreateAsync(userDAL);
            if (success)
            {
                TempData["SuccessMessage"] = "Your account has been created successfully. You can now log in.";
                return RedirectToAction("SignIn");
            }

            ViewData["ErrorMessage"] = "The email address is already taken.";
            model.Client ??= new Client();
            model.RestaurantOwner ??= new RestaurantOwner();
            return View("SignUp", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterRestaurantOwner(SignUpViewModel model)
        {
            SetUserViewData();
            ViewData["ActiveForm"] = "RestaurantOwner";
            bool successForm = true;

            if (!model.IsValid(ModelState, model.RestaurantOwner?.Password))
            {
                successForm = false;
            }

            if (model.RestaurantOwner == null || !ModelState.IsValid)
            {
                successForm = false;
            }

            if (!successForm)
            {
                model.Client ??= new Client();
                model.RestaurantOwner ??= new RestaurantOwner();
                return View("SignUp", model);
            }

            bool success = await model.RestaurantOwner.CreateAsync(userDAL);
            if (success)
            {
                TempData["SuccessMessage"] = "Your account has been created successfully. You can now log in.";
                return RedirectToAction("SignIn");
            }

            ViewData["ErrorMessage"] = "The email address is already taken.";
            model.Client ??= new Client();
            model.RestaurantOwner ??= new RestaurantOwner();
            return View("SignUp", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private void SetUserViewData()
        {
            ViewData["userId"] = HttpContext.Session.GetInt32("userId")?.ToString();
            ViewData["userType"] = HttpContext.Session.GetString("userType");
        }
    }
}
