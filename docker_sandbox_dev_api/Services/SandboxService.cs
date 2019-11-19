using docker_sandbox_dev_api.Contract;
using docker_sandbox_dev_api.Contract.Dtos;
using docker_sandbox_dev_api.Dal;
using docker_sandbox_dev_api.Dal.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Services
{
    public class SandboxService : ISandboxService
    {
        private readonly SandboxDbContext context;
        private readonly IDockerService docker;

        public SandboxService(SandboxDbContext context, IDockerService docker)
        {
            this.context = context;
            this.docker = docker;
        }

        public async Task<Guid> CreateSandbox(Guid userId)
        {
            User user = await this.CreateUserIfNotExisted(userId);

            Console.WriteLine(user.Sandboxes.Count);

            if (user.Sandboxes.Count > 0)
            {
                return user.Sandboxes.First().SandboxId;
            }

            string configDir = $"{userId}/code-server";
            string projDir = $"{userId}/workspace";
            Directory.CreateDirectory(configDir);
            Directory.CreateDirectory(projDir);

            SandboxCreationConfig config = new SandboxCreationConfig()
            {
                Port = user.Port.ToString(),
                ConfigDir = configDir,
                ProjectDir = projDir,
                Password = "123456",
                Image = "codercom/code-server:v2"
            };

            string containerId = await this.docker.CreateContainer(config);
            Sandbox sandbox = new Sandbox()
            {
                SandboxId = Guid.NewGuid(),
                DockerContainerId = containerId,
                UserId = userId,
                DockerContainerPort = config.Port,
                SandboxCreationConfig = JsonConvert.SerializeObject(config)
            };
            context.Sandboxes.Add(sandbox);
            await context.SaveChangesAsync();
            return sandbox.SandboxId;
        }

        private async Task<User> CreateUserIfNotExisted(Guid userId)
        {
            User user = this.context.Users.Include(u => u.Sandboxes).FirstOrDefault(u => u.UserId.Equals(userId));
            if (user == null)
            {
                user = new User()
                {
                    UserId = userId,
                    Port = await GrantUserPortAsync()
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            return user;
        }

        private async Task<int> GrantUserPortAsync()
        {
            List<int> allocatedPort = await this.context.Users.Select(u => u.Port).ToListAsync();

            for (int p = 9000; p < 9999; p++)
            {
                if (!allocatedPort.Contains(p))
                {
                    return p;
                }
            }
            return 10000;
        }

        public async Task DestroySandbox(Guid userId, Guid sandboxId)
        {
            User user = await this.CreateUserIfNotExisted(userId);

            Sandbox sandbox = user.Sandboxes.FirstOrDefault(c => c.SandboxId.Equals(sandboxId));
            if (sandbox == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                await docker.DeleteContainer(sandbox.DockerContainerId);
            }
            finally
            {
                Docker.DotNet.Models.ContainerListResponse response = await this.docker.GetContainer(sandbox.DockerContainerId);
                if (response == null)
                {
                    user.Sandboxes.Remove(sandbox);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<SandboxDto>> GetCreatedSandboxes(Guid userId)
        {
            User user = await this.CreateUserIfNotExisted(userId);
            return user.Sandboxes
                .Select(async sb => new SandboxDto()
                {
                    Id = sb.SandboxId,
                    Port = sb.DockerContainerPort,
                    Status = await docker.GetContainerStatus(sb.DockerContainerId)
                })
                .Select(t => t.Result);
        }

        public async Task<SandboxDto> StartSandbox(Guid userId, Guid sandboxId)
        {
            User user = await this.CreateUserIfNotExisted(userId);

            Sandbox sandbox = user.Sandboxes.FirstOrDefault(c => c.SandboxId.Equals(sandboxId));
            if (sandbox == null)
            {
                throw new KeyNotFoundException();
            }
            Console.WriteLine(sandbox.DockerContainerId);
            await this.docker.StartContainer(sandbox.DockerContainerId);
            return new SandboxDto()
            {
                Id = sandbox.SandboxId,
                Port = sandbox.DockerContainerPort,
                Status = SandboxStatus.Started
            };
        }

        public async Task StopSandbox(Guid userId, Guid sandboxId)
        {

            User user = await this.CreateUserIfNotExisted(userId);

            Sandbox sandbox = user.Sandboxes.FirstOrDefault(c => c.SandboxId.Equals(sandboxId));
            if (sandbox == null)
            {
                throw new KeyNotFoundException();
            }

            await docker.StopContainer(sandbox.DockerContainerId);
        }
    }
}
