using docker_sandbox_dev_api.Contract.Dtos;
using docker_sandbox_dev_api.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Contract
{
    public interface ISandboxService
    {
        Task<IEnumerable<SandboxDto>> GetCreatedSandboxes(Guid userId);
        Task<Guid> CreateSandbox(Guid userId);
        Task<SandboxDto> StartSandbox(Guid userId, Guid sandboxId);
        Task StopSandbox(Guid userId, Guid sandboxId);
        Task DestroySandbox(Guid userId, Guid sandboxId);
    }
}
