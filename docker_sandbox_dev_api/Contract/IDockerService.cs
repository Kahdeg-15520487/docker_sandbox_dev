using docker_sandbox_dev_api.Contract.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Contract
{
    public interface IDockerService
    {
        Task<string> GetFreePort();
        Task<IEnumerable<Docker.DotNet.Models.ContainerListResponse>> GetContainers();
        Task<Docker.DotNet.Models.ContainerListResponse> GetContainer(string containerId);
        Task<string> CreateContainer(SandboxCreationConfig config);
        Task StartContainer(string containerId);
        Task DeleteContainer(string sandboxId);
        Task StopContainer(string dockerContainerId);
        Task<SandboxStatus> GetContainerStatus(string dockerContainerId);
    }
}