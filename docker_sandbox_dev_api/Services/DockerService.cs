using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using docker_sandbox_dev_api.Contract;
using docker_sandbox_dev_api.Contract.Dtos;

namespace docker_sandbox_dev_api.Services
{
    public class DockerService : IDockerService
    {
        static DockerClient client = null;

        public DockerService()
        {
            if (client == null)
            {
                client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
                                    .CreateClient();
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("docker", " run -it -p 8080:8080 -v \"C:/Users/Minh/.local/share/code-server:/home/coder/.local/share/code-server\" -v \"C:/Users/Minh:/home/coder/project\" -e \"PASSWORD=123456\" codercom/code-server:v2");
        }

        public async Task<IEnumerable<ContainerListResponse>> GetContainers()
        {
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = int.MaxValue });
            return containers;
        }

        public async Task<ContainerListResponse> GetContainer(string containerId)
        {
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = int.MaxValue });
            return containers.FirstOrDefault(c => c.ID.Equals(containerId));
        }

        public async Task<string> CreateContainer(SandboxCreationConfig sandboxConfig)
        {
            CreateContainerResponse response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = sandboxConfig.Image,
                Tty = true, // -it
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"8080/tcp", new List<PortBinding>
                        {
                            new PortBinding
                            {
                                HostPort = sandboxConfig.Port
                            }
                        }}
                    }
                },
                Volumes = new Dictionary<string, EmptyStruct>()
                {
                    {$"{sandboxConfig.ConfigDir}:/home/coder/.local/share/code-server",default },
                    {$"{sandboxConfig.ProjectDir}:/home/coder/project",default }
                },
                Env = new List<string>() { $"PASSWORD={sandboxConfig.Password}" }
            });

            return response.ID;

            //ContainerCreationConfig config = new ContainerCreationConfig()
            //{
            //    Port = "9090",
            //    ConfigDir = "C:/Users/Minh/.local/share/code-server",
            //    ProjectDir = "C:/Users/Minh",
            //    Password = "123456",
            //    Image = "codercom/code-server:v2"
            //};

            //ProcessStartInfo processStartInfo = new ProcessStartInfo("docker", " create " + config.ToString());
            //processStartInfo.RedirectStandardOutput = true;
            //Process p = new Process();
            //// Redirect the output stream of the child process.
            //p.StartInfo = processStartInfo;
            //p.Start();
            //string output = p.StandardOutput.ReadToEnd();
            //Console.WriteLine(output);
            //p.WaitForExit();

            //return output;
        }

        public async Task StartContainer(string containerId)
        {
            await client.Containers.StartContainerAsync(containerId, null);
        }

        public async Task DeleteContainer(string containerId)
        {
            await client.Containers.KillContainerAsync(containerId, new ContainerKillParameters());
            await client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
        }

        public async Task<string> GetFreePort()
        {
            return "9090";
        }

        public async Task StopContainer(string containerId)
        {
            await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
        }

        public async Task<SandboxStatus> GetContainerStatus(string containerId)
        {
            ContainerListResponse container = await this.GetContainer(containerId);
            return container.Status switch
            {
                "created" => SandboxStatus.Stopped,
                "stopped" => SandboxStatus.Stopped,
                "paused" => SandboxStatus.Stopped,

                "runing" => SandboxStatus.Started,
                _ => SandboxStatus.Stopped
            };
        }
    }
}
