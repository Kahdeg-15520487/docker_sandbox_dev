using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Dal.Model
{
    public class Container
    {
        public Guid ContainerId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string DockerContainerId { get; set; }
        public string DockerContainerImage { get; set; }
    }
}
