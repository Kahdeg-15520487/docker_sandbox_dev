using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Dal.Model
{
    public class Sandbox
    {
        public Guid SandboxId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string SandboxCreationConfig { get; set; }

        public string DockerContainerId { get; set; }
        public string DockerContainerPort { get; set; }
        public string DockerContainerImage { get; set; }
    }
}
