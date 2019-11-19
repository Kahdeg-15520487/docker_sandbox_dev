using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace docker_sandbox_dev.Controllers
{
    public class SandboxController : Controller
    {
        public IActionResult ReturnToIndex()
        {
            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid sandboxId)
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.DeleteAsync("http://localhost:5003/api/sandbox/" + sandboxId);

            if (response.IsSuccessStatusCode)
            {

            }
            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Start(Guid sandboxId)
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.PostAsync("http://localhost:5003/api/sandbox/start/" + sandboxId, new StringContent(""));

            if (response.IsSuccessStatusCode)
            {

            }
            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Stop(Guid sandboxId)
        {
            string accessToken = await this.HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.PostAsync("http://localhost:5003/api/sandbox/stop/" + sandboxId, new StringContent(""));

            if (response.IsSuccessStatusCode)
            {

            }
            return Redirect("/");
        }
    }
}
