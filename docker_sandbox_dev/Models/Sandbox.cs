using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev.Models
{
    public enum SandboxStatus
    {
        Started,
        Stopped
    }
    public class Sandbox
    {
        public Guid Id { get; set; }
        public string Port { get; set; }
        public SandboxStatus Status { get; set; }
    }
}
