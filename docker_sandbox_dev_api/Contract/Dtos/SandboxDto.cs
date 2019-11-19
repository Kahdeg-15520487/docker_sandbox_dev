using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Contract.Dtos
{
    public enum SandboxStatus
    {
        Started,
        Stopped
    }
    public class SandboxDto
    {
        public Guid Id { get; set; }
        public string Port { get; set; }
        public SandboxStatus Status { get; set; }
    }
}
