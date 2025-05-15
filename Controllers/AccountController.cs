using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;
using Microsoft.AspNetCore.Http;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            SetUserViewData();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewData["ErrorMessage"] = "Email and password are required.";
                return View();
            }

            User user = await TakeAway.Models.User.GetUserAsync(userDAL, email, password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("userId", user.Id);
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
            ViewData["ActiveForm"] = "Client";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(Client client, string confirmPassword, bool conditions)
        {
            SetUserViewData();
            ViewData["ActiveForm"] = "Client";
            bool successForm = true;
            if (!ModelState.IsValid)
            {
                successForm = false;
            }

            if (client.Password != confirmPassword)
            {
                ModelState.AddModelError("", "The passwords do not match.");
                successForm = false;
            }

            if (!conditions)
            {
                ModelState.AddModelError("", "You must accept the terms and conditions.");
                successForm = false;
            }
            if (!successForm)
            {
                return View("SignUp", client);
            }

            bool success = await client.CreateAsync(userDAL);
            if (success)
            {
                return RedirectToAction("SignIn");
            }

            ModelState.AddModelError("", "The email address is already used.");
            return View("SignUp", client);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterRestaurantOwner(RestaurantOwner owner, string confirmPassword, bool conditions)
        {
            SetUserViewData();
            ViewData["ActiveForm"] = "RestaurantOwner";
            bool successForm = true;
            if (!ModelState.IsValid)
            {
                successForm = false;
            }

            if (owner.Password != confirmPassword)
            {
                ModelState.AddModelError("", "The passwords do not match.");
                successForm = false;
            }

            if (!conditions)
            {
                ModelState.AddModelError("", "You must accept the terms and conditions.");
                successForm = false;
            }

            if (!successForm)
            {
                return View("SignUp", owner);
            }

            bool success = await owner.CreateAsync(userDAL);
            if(success)
                return RedirectToAction("SignIn");
            else
                ModelState.AddModelError("", "The email address is already used.");
            return View("SignUp", owner);
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
