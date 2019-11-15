using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using docker_sandbox_dev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace docker_sandbox_dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task Login(string returnUrl = null)
        {
            if (returnUrl == null)
                returnUrl = "/";
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                AuthenticationProperties props = new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                    Items =
                        {
                            { "scheme", "oidc" },
                            { "returnUrl", returnUrl }
                        }
                };
                await HttpContext.ChallengeAsync(props);
            }
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            var ttt = this.HttpContext.AuthenticateAsync().Result;
            ViewData["Message"] = "Secure page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
