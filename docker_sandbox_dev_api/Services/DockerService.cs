using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using docker_sandbox_dev_api.Contract;

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
            //return containers.Select(c => c.ID);
            return containers;
        }

        public async Task<string> CreateContainer(string image)
        {
            //Config config = new Config()
            //{
            //    Tty = true, // -it
            //    ExposedPorts = new Dictionary<string, EmptyStruct>() { { "127.0.0.1:8080:8080", default } }, // -p 127.0.0.1:8080:8080
            //    Volumes = new Dictionary<string, EmptyStruct>()
            //    {
            //        {"C:/Users/Minh/.local/share/code-server:/home/coder/.local/share/code-server",default },
            //        {"C:/Users/Minh:/home/coder/project",default }
            //    },
            //    Image = "codercom/code-server:v2",
            //    Env = new List<string>() { "PASSWORD=123456" }
            //};

            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));

            //CreateContainerResponse response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config
            //));

            //return response;

            var config = new
            {
                Port = "9090",
                ConfigDir = "C:/Users/Minh/.local/share/code-server",
                ProjectDir = "C:/Users/Minh",
                Password = "123456"
            };

            ProcessStartInfo processStartInfo = new ProcessStartInfo("docker", " create -p 9090:8080 -v \"C:/Users/Minh/.local/share/code-server:/home/coder/.local/share/code-server\" -v \"C:/Users/Minh:/home/coder/project\" -e \"PASSWORD=123456\" codercom/code-server:v2");
            processStartInfo.RedirectStandardOutput = true;
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo = processStartInfo;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            p.WaitForExit();

            return output;
        }

        public async Task StartContainer(string containerId)
        {
            await client.Containers.StartContainerAsync(containerId, null);
        }

        public async Task DeleteContainer(string containerId)
        {
            await client.Containers.KillContainerAsync(containerId, null);
        }
    }
}
