using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TakeAway.DAL;
using TakeAway.DAL.Interfaces;
using TakeAway.Models;

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

        public IActionResult SignUp()
        {
            return View();
        }

    }
}
