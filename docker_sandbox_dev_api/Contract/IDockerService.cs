using System.Collections.Generic;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Contract
{
    public interface IDockerService
    {
        Task<IEnumerable<Docker.DotNet.Models.ContainerListResponse>> GetContainers();
        Task<string> CreateContainer(string image);
        Task StartContainer(string containerId);
    }
}