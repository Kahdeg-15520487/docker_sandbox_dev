using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using docker_sandbox_dev_api.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using docker_sandbox_dev_api.Filters;
using docker_sandbox_dev_api.Dal.Model;
using docker_sandbox_dev_api.Contract;

namespace docker_sandbox_dev_api.Controllers
{
    [ApiController]
    [Route("api/sandbox")]
    public class SandboxController : ControllerBase
    {
        private readonly SandboxDbContext context;

        private readonly IDockerService dockerService;

        public SandboxController(SandboxDbContext context, IDockerService dockerService)
        {
            this.context = context;
            this.dockerService = dockerService;
        }

        [HttpGet]
        //[Authorize]
        //[UserMapper]
        public async Task<IEnumerable<Docker.DotNet.Models.ContainerListResponse>> GetCreatedSandbox()
        {
            //Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];
            //User user = this.context.Users.Include(u => u.Containers).FirstOrDefault(u => u.UserId.Equals(userId));
            //return user.Containers.Select(c => c.ContainerId.ToString());
            return await this.dockerService.GetContainers();
        }

        [HttpPost("start/{sandboxId}")]
        public async Task<IActionResult> StartSandbox(string sandboxId)
        {
            await this.dockerService.StartContainer(sandboxId);
            return Ok();
        }

        [HttpPost]
        //[Authorize]
        //[UserMapper]
        public async Task<string> CreateNewSandbox()
        {
            //IEnumerable<string> containers = await this.dockerService.GetContainers();
            //return string.Join(", ", containers);
            return await this.dockerService.CreateContainer("");
        }
    }
}
