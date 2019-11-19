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
using docker_sandbox_dev_api.Contract.Dtos;

namespace docker_sandbox_dev_api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/sandbox")]
    public class SandboxController : ControllerBase
    {
        private readonly ISandboxService sandboxService;

        public SandboxController(ISandboxService sandboxService)
        {
            this.sandboxService = sandboxService;
        }

        [HttpGet]
        [Authorize]
        [UserMapper]
        public async Task<IActionResult> GetCreatedSandbox()
        {
            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];
            try
            {
                return Ok(await this.sandboxService.GetCreatedSandboxes(userId));
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost("start/{sandboxId}")]
        [Authorize]
        [UserMapper]
        public async Task<IActionResult> StartSandbox(Guid sandboxId)
        {
            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];

            try
            {
                return Ok(await this.sandboxService.StartSandbox(userId, sandboxId));
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost("stop/{sandboxId}")]
        [Authorize]
        [UserMapper]
        public async Task<IActionResult> StopSandbox(Guid sandboxId)
        {
            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];

            try
            {
                await this.sandboxService.StopSandbox(userId, sandboxId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [UserMapper]
        public async Task<string> CreateNewSandbox()
        {
            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];
            return (await this.sandboxService.CreateSandbox(userId)).ToString();
        }

        [HttpDelete("{sandboxId}")]
        [Authorize]
        [UserMapper]
        public async Task<IActionResult> DeleteSandbox(Guid sandboxId)
        {
            Guid userId = (Guid)this.Request.HttpContext.Items["UserId"];
            try
            {
                await this.sandboxService.DestroySandbox(userId, sandboxId);
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
            return this.Ok();
        }
    }
}
