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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(Client u)
        {
            User user = await Client.GetUserAsync(userDAL, u.Email, u.Password);
            if (user is Client) {
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("userType", "Client");
                return RedirectToAction("Index", "Home");
            }
            else if (user is RestaurantOwner) {
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("userType", "Restaurateur");
                return RedirectToAction("Index", "Restaurant");
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid e-mail address or password.";
                return View(u);
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(Client client)
        {
            if (client.Password != client.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("SignUp", client);
            }

            if (!client.Conditions)
            {
                ModelState.AddModelError("Conditions", "You must agree to the Terms and Conditions.");
                return View("SignUp", client);
            }

            //bool success = await client.CreateAsync(userDAL);
            if (success)
                return RedirectToAction("SignIn");
            else
                return View("SignUp", owner);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterRestaurantOwner(RestaurantOwner owner)
        {

            if (owner.Password != owner.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View("SignUp", owner);
            }

            if (!owner.Conditions)
            {
                ModelState.AddModelError("Conditions", "You must agree to the Terms and Conditions.");
                return View("SignUp", owner);
            }

            bool success = await owner.CreateAsync(userDAL);
            if(success)
                return RedirectToAction("SignIn");
            else
                return View("SignUp", owner);
        }
    }
}
