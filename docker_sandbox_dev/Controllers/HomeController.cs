using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using docker_sandbox_dev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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
            AuthenticateResult ttt = this.HttpContext.AuthenticateAsync().Result;
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public async Task<IActionResult> CreateSandbox()
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.PostAsync("http://localhost:5003/api/sandbox", new StringContent(""));

            if (response.IsSuccessStatusCode)
            {
                string containerId = await response.Content.ReadAsStringAsync();
                Console.WriteLine(containerId);
                response = await client.PostAsync("http://localhost:5003/api/sandbox/start/" + containerId, new StringContent(""));
                string responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseJson);
                Sandbox sandbox = JsonConvert.DeserializeObject<Sandbox>(responseJson);
                if (response.IsSuccessStatusCode)
                {
                    return View("ide", sandbox);
                }
                else
                {
                    ViewBag.Json = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                ViewBag.Json = "403 - Forbidden";
            }


            return View("json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
